using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public ItemObject item;

    public virtual void Start()
    {
        SaveSystem.Instance.groundItemList.Add(this);
    }

    //variable to check if object is currently being thrown
    private bool isThrown = false;

    public bool getThrownStatus()
    {
        return isThrown;
    }

    public void setThrownStatus(bool isThrown)
    {
        this.isThrown = isThrown;
    }
}
