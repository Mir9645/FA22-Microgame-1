using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoudEffect : MonoBehaviour
{
    public AudioSource Effect;
    
    void Sound()
    {
        Effect.Play();
        Debug.Log("SoundPlayed");
    }
}
