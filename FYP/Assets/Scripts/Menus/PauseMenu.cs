using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //static as we dont want to ref the script, just check from other scripts whether game is paused
    public static bool paused = false;

    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject loadMenuUI;
    public AudioSource clickSound;      //audio source attached to the canvas
    public GameObject tutorials;

    public void ClickSound()
    {
        clickSound.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                ResumeGame();   //if esc pressed while game is paused, resume game
            }
            else
            {
                PauseGame();    //if esc pressed while game is not paused, pause game
            }
        }
    }

    public void ResumeGame()            //also called in the UI ResumeButton
    {
        pauseMenuUI.SetActive(false);       //hide pause menu
        optionsMenuUI.SetActive(false);     //hide options menu
        loadMenuUI.SetActive(false);        //hide load menu
        Time.timeScale = 1f;                //resume time to normal
        paused = false;                     //set paused bool to false
    }

    public void PauseGame()
    {
        bool inTutorial = false;            //bool to check if player is in tutorial

        //loop through Tutorials gameobject's children inside canvas
        for (int i = 0; i < tutorials.transform.childCount; i++)
        {
            //getting the child in current loop iteration
            GameObject currentTutorial = tutorials.gameObject.transform.GetChild(i).gameObject;

            //if child is active, set inTutorial to true
            if (currentTutorial.activeSelf == true)
            {
                inTutorial = true;
            }
        }

        //if player not in tutorial, can open pause menu as usual
        if (inTutorial == false)
        {
            pauseMenuUI.SetActive(true);    //display pause menu
            Time.timeScale = 0f;            //freeze time
            paused = true;                  //set paused bool to true
        }
    }

    public void OpenLoadMenu()
    {
        ClickSound();
        pauseMenuUI.SetActive(false);   //hide pause menu
        loadMenuUI.SetActive(true);     //display load menu
    }

    public void OpenOptionsMenu()
    {
        ClickSound();
        pauseMenuUI.SetActive(false);   //hide pause menu
        optionsMenuUI.SetActive(true);  //display options menu
    }

    //Specifically for during game closing load menu and opening pause menu, doing in the PauseMenu script instead of LoadMenu script for now.
    public void CloseLoadMenu()
    {
        ClickSound();
        loadMenuUI.SetActive(false);     //hide load menu
        pauseMenuUI.SetActive(true);     //display pause menu
    }

    public void QuitToMainMenu()
    {
        ClickSound();
        Debug.Log("Returning to Main Menu");
        ResumeGame();               //close all menus, resume time to normal and return paused bool to false (impt so next load into game has time running normally)
        SceneManager.LoadScene(0);
    }

    public bool getPausedStatus()
    {
        return paused;
    }
}
