using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;

    private void Awake()
    {

        if (SceneManager.GetActiveScene().name != "SampleScene")
        {
            Destroy(this.gameObject);
        }
    }
}
