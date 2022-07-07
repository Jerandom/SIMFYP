using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Molotov Object", menuName = "Inventory System/Items/Molotov")] // allow us to create this default object from unity editor
public class MolotovObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.Molotov;
    }
}
