using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyWater : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && gameObject.name == "HolyWater(Clone)")
        {
            if (collision.GetComponent<BossEnemy_Proto>() != null)
            {
                BossEnemy_Proto boss = collision.GetComponent<BossEnemy_Proto>();
                if (boss != null)
                {
                    boss.state = BossEnemy_Proto.State.STUNNED;    //call die function of EnemyAI script
                    //GameObject onFire = Instantiate(Fire, transform.position, Quaternion.identity);     //instantiate Fire gameobject on collision position
                    //Destroy(onFire, 3f);    //destroy Fire gameobject
                    Destroy(gameObject);    //destroy Molotov object
                }
            }
        }
    }
}
