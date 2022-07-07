using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Bait,
    Molotov,
    Matchstick,
    HolyWater,
    Key,
    HealthPotion,
    Default
}

//base class for creating our items (abstract). We will extend our item objects from this class
public abstract class ItemObject : ScriptableObject
{
    public string Name;
    public int Id;
    public Sprite uiDisplay;   //hold display for item
    public ItemType type;       //store type of item

    [TextArea(15,20)]           //so it is not single line string
    public string description;  //hold description of item
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;

    public Item(ItemObject item)
    {
        Name = item.Name;
        Id = item.Id;
    }
}