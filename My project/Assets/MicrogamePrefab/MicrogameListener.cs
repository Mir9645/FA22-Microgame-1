using UnityEngine;

namespace Egam202
{
    public class MicrogameListener : MonoBehaviour
    {    
        public void OnMicrogameAwake(MicrogameInstance.Difficulty difficulty)
        {
            // Basic information
            gameObject.SendMessage("MicrogameAwake", SendMessageOptions.DontRequireReceiver);
            
            // Difficulty information
            gameObject.SendMessage("MicrogameAwakeDifficulty", difficulty, SendMessageOptions.DontRequireReceiver);        
        }

        public void OnMicrogameStart(float duration, MicrogameInstance.Difficulty difficulty,
            string prompt, MicrogameInstance.PromptType promptType)
        {
            // Basic information
            gameObject.SendMessage("MicrogameStart", SendMessageOptions.DontRequireReceiver);
            
            // Timer information
            gameObject.SendMessage("MicrogameStartDuration", duration, SendMessageOptions.DontRequireReceiver);

            // Prompt information
            gameObject.SendMessage("MicrogameStartPrompt", prompt, SendMessageOptions.DontRequireReceiver);
            gameObject.SendMessage("MicrogameStartPromptType", promptType, SendMessageOptions.DontRequireReceiver);
            
            // Difficulty information
            gameObject.SendMessage("MicrogameStartDifficulty", difficulty, SendMessageOptions.DontRequireReceiver);
        }

        public void OnMicrogameTimeOut(MicrogameInstance instance)
        {
            // Basic information
            gameObject.SendMessage("MicrogameTimeOut", instance, SendMessageOptions.DontRequireReceiver);
        }
    }
}