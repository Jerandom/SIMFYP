using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject bossEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bossEnemy.gameObject.SetActive(true);
    }
}