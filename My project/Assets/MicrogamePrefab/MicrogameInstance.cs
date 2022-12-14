using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Egam202
{
    public class MicrogameInstance : MonoBehaviour
    {
        // Microgame information
        public enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }

        [System.Serializable] 
        public class MicrogameSetting
        {
            public Difficulty difficulty;
            public float duration = 10f;
        }

        [Header("Difficulty Info")]
        [SerializeField] private MicrogameSetting[] _settings = null;

        // Time over behavior
        public enum TimeStartType
        {
            Immediate,
            WaitForPrompt                       
        }

        public enum TimeOverType
        {
            Lose,
            Win,
            Notify
        }

        [Header("Time Info")]
        [SerializeField] private TimeStartType _timeStartType = TimeStartType.Immediate;
        [SerializeField] private TimeOverType _timeOverType = TimeOverType.Lose;        

        // Prompt information
        public enum PromptType
        {
            Default,
            StayOnTop,
            StayOnBottom
        }

        [Header("Prompt Info")]
        [SerializeField] private string _microgamePrompt = "Command!";
        [SerializeField] private float _promptDuration = 1f;
        [SerializeField] private PromptType _promptType = PromptType.Default;

        // Timer information
        private float _timer = 10f;
        public float timeLeft 
        {
            get { return _timer; }
        }

        private float _originalTimer = 10f;
        public float timeTotal 
        {
            get { return _originalTimer; }
        }

        public bool isTimerActive
        {
            get { return _timer >= 0; }
        }

        public bool isPromptVisible 
        {
            get 
            { 
                bool isVisible = (timeTotal - timeLeft) <= _promptDuration;
                switch (_timeStartType)
                {
                    case TimeStartType.WaitForPrompt:
                        isVisible = timeLeft > timeTotal;
                        break;
                }
                return isVisible;
            }                
        }

        // Exit flow
        private Coroutine _exitRoutine = null;
        private bool _hasMicrogameStarted = false;

        public delegate void MicrogameEndHandler(bool isWin);
        public event MicrogameEndHandler OnMicrogameEnded = delegate {};

        // Debug information
        public enum DebugType
        {
            RestartOnTimeout,
            DoNothing,            
        }

        [Header("Debug Info")]
        public Difficulty _DEBUG_Difficulty = Difficulty.Easy;
        public DebugType _DEBUG_Type = DebugType.RestartOnTimeout;
        private bool _isDebugFlow = false;
        
        void Start()
        {
            // Broadcast a message the game has started?
            StartMicrogame(_DEBUG_Difficulty, true);
        }

        public void StartMicrogame(Difficulty difficulty, bool isDebug = false)
        {
            // Refuse to restart
            if (!_hasMicrogameStarted)
            {
                _hasMicrogameStarted = true;            
                _isDebugFlow = isDebug;

                // Find the matching settings
                MicrogameSetting setting = null;
                for (int i = 0; i < _settings.Length; i++)
                {
                    if (difficulty == _settings[i].difficulty)
                    {
                        setting = _settings[i];
                        break;
                    }
                }

                _originalTimer = 10f;
                if (setting != null)
                {
                    _originalTimer = setting.duration;
                }
                
                // Start the timer
                _timer = _originalTimer;

                // Buffer time for the prompt?
                switch (_timeStartType)
                {
                    case TimeStartType.WaitForPrompt:
                        _timer += _promptDuration;
                        break;
                }

                // Broadcast a message that we're ready to go
                MicrogameListener[] listeners = GameObject.FindObjectsOfType<MicrogameListener>();
                
                for (int i = 0; i < listeners.Length; i++)
                {
                    // This gives game the option to override the prompt information
                    listeners[i].OnMicrogameAwake(difficulty);
                }
                
                for (int i = 0; i < listeners.Length; i++)
                {
                    listeners[i].OnMicrogameStart(_originalTimer, difficulty, _microgamePrompt, _promptType);
                }
            }
        }

        void Update()
        {
            // Decrement the timer
            if (isTimerActive)
            {
                _timer -= Time.deltaTime;

                // What should we do if the timer runs out?
                if (!isTimerActive)
                {
                    switch (_timeOverType)
                    {
                        case TimeOverType.Lose:
                            OnGameLose(true);
                            break;

                        case TimeOverType.Win:
                            OnGameWin(true);
                            break;

                        case TimeOverType.Notify:
                            // Ask someone else to report?
                            OnGameTimerOut();
                            break;
                    }
                }
            }
        }

        public void OverridePrompt(string newPromptString)
        {
            _microgamePrompt = newPromptString;
        }

        private void OnGameTimerOut()
        {
            MicrogameListener[] listeners = GameObject.FindObjectsOfType<MicrogameListener>();
            for (int i = 0; i < listeners.Length; i++)
            {
                listeners[i].OnMicrogameTimeOut(this);
            }
        }

        public void OnGameLose(bool isInstantExit = false)
        {
            // Dock a life, move back to the main flow
            ExitMicrogame(false, isInstantExit);
        }

        public void OnGameWin(bool isInstantExit = false)
        {
            // Move back to the main flow
            ExitMicrogame(true, isInstantExit);
        }

        private void ExitMicrogame(bool isWin, bool isInstantExit)
        {
            // Only exit once
            if (_exitRoutine == null)
            {
                _exitRoutine = StartCoroutine(ExecuteExitMicrogame(isWin, isInstantExit));
            }
            else
            {
                if (isWin)
                {
                    Debug.LogWarning("MicrogameInstance.cs: You might be calling OnGameWin too many times...");
                }
                else
                {
                    Debug.LogWarning("MicrogameInstance.cs: You might be calling OnGameLose too many times...");
                }
            }
        }

        private IEnumerator ExecuteExitMicrogame(bool isWin, bool isInstantExit)
        {
            // Wait for the microgame to finish?
            if (!isInstantExit)
            {
                // Wait for the game to end
                while (isTimerActive)
                {
                    yield return null;
                }
            }

            // Now we're ready to exit back to the main flow
            _exitRoutine = null;

            // Debug flow?  Restart the scene so we can play again...)
            if (_isDebugFlow)
            {
                switch (_DEBUG_Type)
                {
                    case DebugType.RestartOnTimeout:
                        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                        break;

                    case DebugType.DoNothing:
                        if (isWin)
                        {
                            Debug.Log("Win! (Time is up)");
                        }
                        else
                        {
                            Debug.Log("Lose... (Time is up)");
                        }
                        break;
                }
            }
            else
            {
                OnMicrogameEnded(isWin);
            }
        }
    }
}