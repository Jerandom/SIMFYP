using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class SaveSystem : MonoBehaviour
{
    public static int saveID;

    private static SaveSystem instance;

    GameObject player;

   [SerializeField]
    public List<EnemyAI> enemyAIList { get; private set; }
    public List<GroundItem> groundItemList { get; private set; }

    public List<hpPot> potionDataList { get; private set; }

    [SerializeField]
    public GameObject bossFinalVersion { get; private set; }

    private String action;

    [SerializeField]
    private SaveGameUI[] saveslots;

    [SerializeField]
    private GameObject dialogue;
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    private SaveGameUI current;

    public GameObject menu;

    public static SaveSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SaveSystem>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Constants.Player);
        enemyAIList = new List<EnemyAI>();
        groundItemList = new List<GroundItem>();
        bossFinalVersion = GameObject.FindGameObjectWithTag("Boss");
        potionDataList = new List<hpPot>();
        foreach (SaveGameUI ui in saveslots)
        {
            //Showing Saved Files
            showSavedFiles(ui);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Load"))
        {
            load(saveslots[PlayerPrefs.GetInt("Load")]);
            PlayerPrefs.DeleteKey("Load");
        }
    }

    public void showDialogue(GameObject gameObject)
    {
        action = gameObject.name;
        switch (action)
        {
            case "Load":
                dialogueText.text = "Load Game?";
                break;
            case "Save":
                dialogueText.text = "Save Game?";
                break;
            case "Delete":
                dialogueText.text = "Delete Game?";
                break;
        }
        current = gameObject.GetComponentInParent<SaveGameUI>();
        dialogue.SetActive(true);
    }

    public void executeAction()
    {
        switch (action)
        {
            case "Load":
                LoadTheScene(current);
                break;
            case "Save":
                save(current);
                break;
            case "Delete":
                Delete(current);
                break;
            default:
                break;
        }
    }

    public void closeDialogue()
    {
        dialogue.SetActive(false);
    }

    public void Delete(SaveGameUI saveGameUI)
    {
        File.Delete(Application.persistentDataPath + "/" + saveGameUI.gameObject.name + ".dat");
        closeDialogue();
        saveGameUI.hideInfo();
    }

    void showSavedFiles(SaveGameUI saveGameUI)
    {
        if(File.Exists(Application.persistentDataPath + "/" + saveGameUI.gameObject.name + ".dat"))
        {
            saveGameUI.Visuals.SetActive(true);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + saveGameUI.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            saveGameUI.showInfo(data);
        }
    }

    public void save(SaveGameUI saveGameUI)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + saveGameUI.gameObject.name + ".dat", FileMode.Create);

            SaveData data = new SaveData();

            data.myScene = SceneManager.GetActiveScene().name;

            SavePlayer(data);

            SaveEnemies(data);

            saveItems(data);

            savePotions(data);

            saveBoss(data);

            bf.Serialize(file, data);

            file.Close();

            showSavedFiles(saveGameUI);

            closeDialogue();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    private void SavePlayer(SaveData data)
    {
        if(player != null)
        {
            data.currentPlayerData = new PlayerData(player.GetComponent<PlayerHealth>().getPlayerHP()
                                            , player.GetComponent<PlayerHealth>().getMaxHP()
                                            , player.transform.position.x
                                            , player.transform.position.y);
        }
    }

    private void SaveEnemies(SaveData data)
    {
        for(int i = 0; i < enemyAIList.Count; i++)
        {
            string waypoints = "";
            if (enemyAIList[i].waypointTargets.Length > 0)
            {
                for (int j = 0; j < enemyAIList[i].waypointTargets.Length; j++)
                {
                    if(j == 0)
                    {
                        waypoints += enemyAIList[i].waypointTargets[j].transform.position.ToString();
                    }
                    else
                    {
                        waypoints += "_" + enemyAIList[i].waypointTargets[j].transform.position.ToString();
                    }
                }
            }
            data.enemyData.Add(new EnemyData(enemyAIList[i].getType()
                                , enemyAIList[i].transform.position.x
                                , enemyAIList[i].transform.position.y
                                , waypoints
                                , enemyAIList[i].name));
        }
        
    }

    private void saveItems(SaveData data)
    {
        for(int i = 0; i < groundItemList.Count; i++)
        {
            data.itemData.Add(new itemData(groundItemList[i].name
                                , groundItemList[i].transform.position.x
                                , groundItemList[i].transform.position.y));
        }
    }

    private void savePotions(SaveData data)
    {
        for (int i = 0; i < potionDataList.Count; i++)
        {
            data.potionDataL.Add(new potionData(potionDataList[i].name
                                , potionDataList[i].transform.position.x
                                , potionDataList[i].transform.position.y));
        }
    }

    private void saveBoss(SaveData data)
    {
        if (bossFinalVersion != null)
        {
            data.bossData = new BossData(bossFinalVersion.name
                                        , bossFinalVersion.transform.position.x
                                        , bossFinalVersion.transform.position.y);
        }
        else
        {
            return;
        }
    }

    public void LoadTheScene(SaveGameUI saveGameUI)
    {
        if (File.Exists(Application.persistentDataPath + "/" + saveGameUI.gameObject.name + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + saveGameUI.gameObject.name + ".dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            closeDialogue();
            PlayerPrefs.SetInt("Load", saveGameUI.theIndex);
            SceneManager.LoadScene(data.myScene);
        }
    }

    public void load(SaveGameUI saveGameUI)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/" + saveGameUI.gameObject.name + ".dat", FileMode.Open);

            SaveData data =  (SaveData)bf.Deserialize(file);

            file.Close();

            LoadPlayer(data);

            LoadEnemies(data);

            LoadItems(data);

            loadPotions(data);

            LoadBoss(data);

            menu.GetComponent<PauseMenu>().ResumeGame();
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            //Runs
            Delete(saveGameUI);
            PlayerPrefs.DeleteKey("Load");
        }
    }

    private void LoadPlayer(SaveData data)
    {
        if (player != null)
        {
            player.GetComponent<PlayerHealth>().setPlayerHealth(data.currentPlayerData.currentHP);
            player.GetComponent<PlayerHealth>().setMaxHp(data.currentPlayerData.maxHP);
            player.transform.position = new Vector2(data.currentPlayerData.positionx, data.currentPlayerData.positiony);
        }
    }

    private void LoadEnemies(SaveData data)
    {
        foreach (EnemyAI enemyAI in enemyAIList)
        {
            if (enemyAI != null)
            {
                Destroy(enemyAI.GetComponent<EnemyAI>().getOutterFOV().gameObject);
                Destroy(enemyAI.GetComponent<EnemyAI>().getInnerFOV().gameObject);
                for(int i = 0; i < enemyAI.waypointTargets.Length; i++)
                {
                    Destroy(enemyAI.waypointTargets[i].gameObject);
                }
                Destroy(enemyAI.gameObject);
            }
        }

        enemyAIList.Clear();

        if (data.enemyData.Count > 0)
        {
            for(int i = 0; i < data.enemyData.Count; i++)
            {
                string[] waypointString = data.enemyData[i].waypoints.Split('_');
                GameObject tmp = null;
                switch (data.enemyData[i].type)
                {
                    case Constants.Zombie:
                        tmp = Instantiate(Resources.Load("Zombie") as GameObject);
                        break;

                    case Constants.Skeleton:
                        tmp = Instantiate(Resources.Load("Skeleton") as GameObject);
                        break;

                    case Constants.Spider:
                        tmp = Instantiate(Resources.Load("Spider") as GameObject);
                        break;
                }
                if (tmp != null)
                {
                    tmp.name = data.enemyData[i].name;
                    tmp.transform.position = new Vector2(data.enemyData[i].posX, data.enemyData[i].posY);
                    tmp.GetComponent<EnemyAI>().Load2(waypointString);
                }
            }
        }
    }

    private void LoadItems(SaveData data)
    {
        foreach (GroundItem groundItem in groundItemList)
        {
            if (groundItem != null)
            {
                Destroy(groundItem.gameObject);
            }
        }
        groundItemList.Clear();

        if(data.itemData.Count > 0)
        {
            for(int i = 0; i < data.itemData.Count; i++)
            {
                GameObject tmp = null;
                switch (data.itemData[i].name)
                {
                    case Constants.Bone:
                        tmp = Instantiate(Resources.Load(Constants.Bone) as GameObject);
                        break;

                    case Constants.Fly:
                        tmp = Instantiate(Resources.Load(Constants.Fly) as GameObject);
                        break;

                    case Constants.Matchstick:
                        tmp = Instantiate(Resources.Load(Constants.Matchstick) as GameObject);
                        break;

                    case Constants.Meat:
                        tmp = Instantiate(Resources.Load(Constants.Meat) as GameObject);
                        break;

                    case Constants.Molotov:
                        tmp = Instantiate(Resources.Load(Constants.Molotov) as GameObject);
                        break;
                }
                if (tmp != null)
                {
                    tmp.name = data.itemData[i].name;
                    tmp.transform.position = new Vector2(data.itemData[i].posX, data.itemData[i].posY);
                }
            }
        }

    }

    private void loadPotions(SaveData data)
    {
        foreach (hpPot pot in potionDataList)
        {
            if (pot != null)
            {
                Destroy(pot.gameObject);
            }
        }
        potionDataList.Clear();

        if (data.potionDataL.Count > 0)
        {
            for (int i = 0; i < potionDataList.Count; i++)
            {
                GameObject tmp = null;
                tmp = Instantiate(Resources.Load(Constants.HealthPotion) as GameObject);
            }

        }
    }

    private void LoadBoss(SaveData data)
    {
        if (bossFinalVersion != null)
        {
            bossFinalVersion.name = data.bossData.name;
            bossFinalVersion.transform.position = new Vector2(data.bossData.posX, data.bossData.posY);
        }
        else
        {
            return;
        }
    }

    public Vector2 stringToVector(string value)
    {
        //value = (1, 2, 3)
        value = value.Trim(new char[] { '(', ')' });
        //value = 1, 2, 3
        value.Replace(" ", "");
        //value = 1,2,3
        string[] pos = value.Split(',');
        //pos[0] = 1, pos[1] = 2, pos[2] = 3


        return new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
    }
}