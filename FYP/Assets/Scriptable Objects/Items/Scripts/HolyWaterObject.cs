using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HolyWater Object", menuName = "Inventory System/Items/HolyWater")] // allow us to create this default object from unity editor
public class HolyWaterObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.HolyWater;
    }
}
