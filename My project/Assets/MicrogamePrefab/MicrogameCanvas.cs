using UnityEngine;
using UnityEngine.UI;

namespace Egam202
{
    public class MicrogameCanvas : MonoBehaviour
    {
        // Timer information
        [SerializeField] private Slider _slider = null;

        // Instance iinformation
        private MicrogameInstance _instance = null;

        // Start is called before the first frame update
        void Start()
        {
            // Find the manager
            _instance = GameObject.FindObjectOfType<MicrogameInstance>();
        }
        
        // Update is called once per frame
        void Update()
        {
            float timeLeft = _instance.timeLeft;
            float timeTotal = _instance.timeTotal;
            float timeElapsed = timeTotal - timeLeft;

            // Update the timer
            float timerInterp = Mathf.Clamp01(_instance.timeLeft / _instance.timeTotal);
            _slider.SetValueWithoutNotify(timerInterp);
        }
    }
}