using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bait Object", menuName = "Inventory System/Items/Bait")] // allow us to create this default object from unity editor
public class BaitObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Bait;
    }
}
