using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
//Since Unity doesnt serialize dictionaries, ISerializationCallbackReceiver used to create the dictionary
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Items;  //array of all items that exist within game
    //make dictionary to import item and return id of item
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {   
        for (int i = 0; i < Items.Length; i++)      //loop through all our items
        {
            Items[i].Id = i;
            GetItem.Add(i, Items[i]);
            //now everytime unity serializes the scriptable object, it will automatically populate the Dictionary with reference values based off
            //our Items array created in unity editor 
        } 
    }

    //not used
    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemObject>();
    }
}
