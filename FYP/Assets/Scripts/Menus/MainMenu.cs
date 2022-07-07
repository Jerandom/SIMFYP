using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource clickSound;      //audio source attached to the canvas
    public GameObject optionsMenuUI;
    public GameObject loadMenuUI;

    public void ClickSound()
    {
        clickSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            loadMenuUI.SetActive(false);     //hide load menu
            optionsMenuUI.SetActive(false);  //hide options menu
        }
    }

    public void NewGame()
    {
        ClickSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   //load next scene based off index when building
    }

    public void OpenLoadMenu()
    {
        ClickSound();
        loadMenuUI.SetActive(true);  //display load menu
    }

    public void OpenOptionsMenu()
    {
        ClickSound();
        optionsMenuUI.SetActive(true);  //display options menu
    }

    public void QuitGame()
    {
        ClickSound();
        Debug.Log("You are Quitting Graverobber");
        Application.Quit();
    }
}
