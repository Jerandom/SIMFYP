using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    public AudioSource clickSound;      //audio source attached to the canvas
    public GameObject pauseMenuUI;
    public GameObject loadMenuUI;

    public void ClickSound()
    {
        clickSound.Play();
    }

    public void Save()
    {
        ClickSound();
    }


    public void Load()
    {
        ClickSound();
    }

    public void CloseLoadMenu()
    {
        ClickSound();
        loadMenuUI.SetActive(false);  //display load menu
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
