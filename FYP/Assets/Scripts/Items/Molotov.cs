using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : GroundItem
{
    public GameObject Fire;

    void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.tag != "Player" && gameObject.name == "Molotov(Clone)")
        {
            if(collision.GetComponent<SkeletonEnemyAI>() != null)
            {
                SkeletonEnemyAI enemy = collision.GetComponent<SkeletonEnemyAI>();
                if (enemy != null)
                {
                    enemy.Die();    //call die function of EnemyAI script
                    ScoreManager.instance.subtractPoint(20);    //subtract points from score manager
                    GameObject onFire = Instantiate(Fire, transform.position, Quaternion.identity);     //instantiate Fire gameobject on collision position
                    Destroy(onFire, 3f);    //destroy Fire gameobject
                    Destroy(gameObject);    //destroy Molotov object
                }
            }
            else if (collision.GetComponent<ZombieEnemyAI>() != null)
            {
                ZombieEnemyAI enemy = collision.GetComponent<ZombieEnemyAI>();
                if (enemy != null)
                {
                    enemy.Die();    //call die function of EnemyAI script
                    ScoreManager.instance.subtractPoint(20);    //subtract points from score manager
                    GameObject onFire = Instantiate(Fire, transform.position, Quaternion.identity);     //instantiate Fire gameobject on collision position
                    Destroy(onFire, 3f);    //destroy Fire gameobject
                    Destroy(gameObject);    //destroy Molotov object
                }
            }
            else if (collision.GetComponent<SpiderEnemyAI>() != null)
            {
                SpiderEnemyAI enemy = collision.GetComponent<SpiderEnemyAI>();
                if (enemy != null)
                {
                    enemy.Die();    //call die function of EnemyAI script
                    ScoreManager.instance.subtractPoint(20);    //subtract points from score manager
                    GameObject onFire = Instantiate(Fire, transform.position, Quaternion.identity);     //instantiate Fire gameobject on collision position
                    Destroy(onFire, 3f);    //destroy Fire gameobject
                    Destroy(gameObject);    //destroy Molotov object
                }
            }
               //if molotov collides with GameObject with EnemyAI script..
            

            /*
            //if want to make molotov explode upon colliding with walls/enemies
            if (collision.tag == "Wall" || collision.tag == "Enemy")
            {
                GameObject onFire2 = Instantiate(Fire, transform.position, Quaternion.identity);     //instantiate Fire gameobject on collision position
                Destroy(onFire2, 3f);    //destroy Fire gameobject
                Destroy(gameObject);    //destroy Molotov object\
            }
            */
        }
    }
}
