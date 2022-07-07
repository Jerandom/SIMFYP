using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Default")] // allow us to create this default object from unity editor
public class DefaultObject : ItemObject     //extend ItemObject
{
    public void Awake()
    {
        type = ItemType.Default;
    }
}
