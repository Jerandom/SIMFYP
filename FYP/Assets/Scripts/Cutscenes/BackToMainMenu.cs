using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("MainMenuScreen");
    }
}
