using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorial;
    Collider2D collider;
    public bool inTutorial = false;    //to check if player has a tutorial window open

    void Start()
    {
        //Fetch tutorial collider
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && inTutorial == true)
        {
            tutorial.SetActive(false);                  //close/disable tutorial window
            Time.timeScale = 1f;                        //resume time
            inTutorial = false;
        }  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")                  //check that collided with player   
        {
            tutorial.SetActive(true);                   //display tutorial
            Time.timeScale = 0f;                        //freeze time
            collider.enabled = false;                   //disable collider to prevent further popups
            inTutorial = true;
        }
    }
}
