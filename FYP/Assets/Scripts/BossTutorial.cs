using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//OUTDATED SCRIPT DO NOT USE

public class BossTutorial : MonoBehaviour
{
    public GameObject bossEnemy;
    public GameObject bossTutorial;
    Collider2D collider;
    public static bool inTutorial = false;    //to check if player has a tutorial window open

    void Start()
    {
        //Fetch BossTrigger's collider
        collider = GetComponent<Collider2D>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && inTutorial == true)
        {
            bossTutorial.gameObject.SetActive(false);   //close/disable boss tutorial
            Time.timeScale = 1f;                        //resume time
            inTutorial = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bossEnemy.gameObject.SetActive(true);

        bossTutorial.gameObject.SetActive(true);    //display boss tutorial
        Time.timeScale = 0f;                        //freeze time
        collider.enabled = false;
        inTutorial = true;
    }
}
