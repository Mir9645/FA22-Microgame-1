using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject Hero;
    public GameObject Left;
    public GameObject Right;
    private bool MoveLeft;
    private bool MoveRight;

    // Start is called before the first frame update
    void Start()
    {
        Hero.SetActive(true);
        Left.gameObject.SetActive(true);
        Right.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetKeyDown(KeyCode.A)) && Left.activeInHierarchy == false)
        {
            Right.gameObject.SetActive(false);
            Left.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetKeyDown(KeyCode.D)) && Right.activeInHierarchy == false)
        {
            Left.gameObject.SetActive(false);
            Right.gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Hero.SetActive(false);
        FindObjectOfType<GameContorller>().GameOver();
    }

    private void FixedUpdate()
    {
        
    }
}
