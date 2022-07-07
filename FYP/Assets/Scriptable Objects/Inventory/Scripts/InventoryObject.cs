using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject     //ISerializationCallbackReceiver removed since not using JSON
{
    public string savePath;     //path that we want to save our file to
    public ItemDatabaseObject database;
    public Inventory Container;

    /*
    //make sure our database variable is set
    private void OnEnable()
    {
        //make a check to ensure this code is only being run in unity editor
#if UNITY_EDITOR
        //every time scriptable object runs this function, automatically sets database to where the file is inside the editor
        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
#else
        database = Resources.Load<ItemDatabaseObject>("Database");
#endif
    }
    */

    public void AddItem(Item _item, int _amount)
    {
        //find out whether the item we are trying to add in our inventory or not
        for (int i = 0; i < Container.Items.Count; i++)
        {
            if (Container.Items[i].item.Id == _item.Id)
            {
                //loop true our Container(inventory), if item matching, add to that amount (Stacking) and return.
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        //But if no items matching currently in inventory, create a new item
        Container.Items.Add(new InventorySlot(_item.Id, _item, _amount));    //use database's GetID to get ID and populate into InventorySlot
    }

    public void StartInventory()
    {
        //Container.Items.Add(new InventorySlot(3, Meat, 0));
    }

    [ContextMenu("Save")]
    //functions for saving and loading inventory
    public void Save()
    {
        /*
        //use binary formatter in JSON utility
        //use JSON utility to serialize scriptable object out to a string
        string saveData = JsonUtility.ToJson(this, true);   //true for prettyPrint to format output for readability

        //use binary formatter and filestream to create a file and write the stream into file and save to given location
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        //Application.persistentDataPath allows us to save files out to persistent path over different types of devices
        FileStream fileStream = File.Create(string.Concat(Application.persistentDataPath, savePath));
        binaryFormatter.Serialize(fileStream, saveData);
        fileStream.Close();
        */

        //instead of using JSON..
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        //check if we have a save file to load from
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            /*
            //if file exists in save path, open the file
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);

            //once file is open, convert file back to Scriptable Object
            JsonUtility.FromJsonOverwrite(binaryFormatter.Deserialize(fileStream).ToString(), this);    //pass in fileStream and paste to this(our scriptable object)

            //close file
            fileStream.Close();
            */

            //instead of using JSON..
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Container = (Inventory)formatter.Deserialize(stream);
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new Inventory();
    }

    /* As no longer using JSON
    public void OnAfterDeserialize()
    {
        //as soon as unity needs to serialize a changed object, look through each item in container and repopulate item slot to ensure its the same item (match by id)
        for (int i = 0; i < Container.Items.Count; i++)   //for each item in our container
        {
            Container.Items[i].item = database.GetItem[Container.Items[i].id];
        }
    }

    //not used
    public void OnBeforeSerialize()
    {
    }
    */
}

[System.Serializable]
public class Inventory
{
    //List of InventorySlot to hold our items
    public List<InventorySlot> Items = new List<InventorySlot>();
}

[System.Serializable]       //when adding this class to an object, shows up in editor
public class InventorySlot
{
    public int id;
    public Item item;
    public int amount;

    //constructor
    public InventorySlot(int _id, Item _item, int _amount)
    {
        id = _id;
        item = _item;
        amount = _amount;
    }

    //add amount of object to InventorySlot
    public void AddAmount(int value)
    {
        amount += value;
    }
}
