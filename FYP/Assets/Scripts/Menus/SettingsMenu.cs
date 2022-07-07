using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    //reference our audio mixer MainVolumeMixer
    public AudioMixer audioMixer;

    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;

    public TMPro.TMP_Dropdown resolutionDropDown;     //to reference ResolutionDropDown in canvas
    public Toggle fullscreenToggle;                   //to reference FullscreenToggle in canvas
    Resolution[] resolutions;                         //Resolution is default in unity
    
    public AudioSource clickSound;                    //audio source attached to the canvas

    public void ClickSound()
    {
        clickSound.Play();
    }

    void Start()
    {
        //need to get what resolutions are available on the current platform game is being played on
        resolutions = Screen.resolutions;       //Screen.resolutions is default in unity

        resolutionDropDown.ClearOptions();      //clear current options in the dropdown

        //turn array of resolutions into list of strings
        List<string> resolutionList = new List<string>();
        int currentResolutionIndex = 0;     //to find our current resolution and set to default
        for (int i = 0; i < resolutions.Length; i++)
        {
            string addToList = resolutions[i].width + " * " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";
            resolutionList.Add(addToList);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                //if current loop iteration resolution matches current screen resolution, mark down the index
                currentResolutionIndex = i;
            }
        }   //we now have a List of strings containing the resolutions

        //add this list of strings to our ResolutionDropDown
        resolutionDropDown.AddOptions(resolutionList);

        //update default resolution displayed to our marked resolution
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();

        //set default fullscreen boolean
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    //VolumeSlider and MainVolumeMixer
    public void SetVolume(float volume)
    {
        //set exposed param mixerVolume in MainVolumeMixer to volume parameter from VolumeSlider
        audioMixer.SetFloat("mixerVolume", volume);
    }

    //ResolutionDropDown
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];   //assign parameter selected in ResolutionDropDown to variable
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);   //set appropriate width, height, with currently togged FullScreenToggle
    }

    //FullScreenToggle and inbuilt unity setting
    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;               //inbuilt Unity method to set Screen setting according to parameter (from FullscreenToggle)
    }

    //Not used for now
    //GraphicsDropDown and Edit -> Project Settings -> Quality
    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);       //inbuilt Unity method to set Project Settings Quality to parameter (from GraphicsDropDown)
    }

    public void OpenPauseMenu()
    {
        ClickSound();
        pauseMenuUI.SetActive(true);   //display pause menu
        optionsMenuUI.SetActive(false);  //hide options menu
    }

    public void CloseOptionsMenu()
    {
        ClickSound();
        optionsMenuUI.SetActive(false);  //hide options menu
    }
}
