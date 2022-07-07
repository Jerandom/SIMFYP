using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpPot : MonoBehaviour
{
    private void Start()
    {
        SaveSystem.Instance.potionDataList.Add(this);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerHealth>().heal(50);
            SaveSystem.Instance.potionDataList.Remove(this);
            Destroy(this.gameObject);
        }
    }
}
