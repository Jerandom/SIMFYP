using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HealthPotion Object", menuName = "Inventory System/Items/HealthPotion")] // allow us to create this default object from unity editor
public class HealthPotionObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.HealthPotion;
    }
}
