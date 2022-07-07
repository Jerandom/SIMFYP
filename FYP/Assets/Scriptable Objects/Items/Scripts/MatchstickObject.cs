using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Matchstick Object", menuName = "Inventory System/Items/Matchstick")] // allow us to create this default object from unity editor
public class MatchstickObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Matchstick;
    }
}
