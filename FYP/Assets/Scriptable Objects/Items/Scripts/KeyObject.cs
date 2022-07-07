using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Key Object", menuName = "Inventory System/Items/Key")] // allow us to create this default object from unity editor
public class KeyObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Key;
    }
}
