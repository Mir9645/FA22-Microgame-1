using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float Timevalue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Timevalue > 0)
        {
            Timevalue -= Time.deltaTime;
        }
        else
        {
            Timevalue = 0;
        }
    }
}
