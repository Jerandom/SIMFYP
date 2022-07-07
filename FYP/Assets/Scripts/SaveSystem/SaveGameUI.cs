 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveGameUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI dateTime; 

    [SerializeField]
    private TextMeshProUGUI Scene;

    [SerializeField]
    public GameObject Visuals;

    [SerializeField]
    private int index;

    public int theIndex
    {
        get { return index; }
    }

    private void Awake()
    {
       
    }

    public void showInfo(SaveData saveData)
    {
        Visuals.SetActive(true);
        dateTime.text = "Date: " + saveData.MyDateTime.ToString("dd/MM/yyy") + " - Time: " + saveData.MyDateTime.ToString("H:m");
        Scene.text = saveData.myScene;
    }

    public void hideInfo()
    {
        Visuals.SetActive(false);
    }
}
