using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchstickUI : MonoBehaviour
{
    public GameObject Fire;
    private bool hasMatchstick = false;      //to check if can burn baited enemy
    public GameObject matchstickUI;
    private GameObject target;
    public InventoryObject inventory;

    void Start()
    {
        matchstickUI.SetActive(false);      //Initialize matchstick panel as false
    }

    public void useMatchstick()
    {
        EnemyAI enemy = target.GetComponent<EnemyAI>();
        ZombieEnemyAI zombieEnemy = target.GetComponent<ZombieEnemyAI>();
        SkeletonEnemyAI skeletonEnemy = target.GetComponent<SkeletonEnemyAI>();
        SpiderEnemyAI spiderEnemy = target.GetComponent<SpiderEnemyAI>();

        if (enemy != null)
        {
            for (int i = 0; i < inventory.Container.Items.Count; i++)                                    //loop through inventory items
            {
                if (inventory.Container.Items[i].item.Name == "Matchstick" && inventory.Container.Items[i].amount > 0)  //find matchstick and check if has more than 0
                {

                    enemy.Die();                    //call die function of EnemyAI script
                    ScoreManager.instance.addPoint(5);  //add point to score manager
                    GameObject onFire = Instantiate(Fire, target.transform.position, Quaternion.identity);     //instantiate Fire gameobject on collision position
                    Destroy(onFire, 3f);            //destroy Fire gameobject

                    inventory.Container.Items[i].amount = inventory.Container.Items[i].amount - 1;      //minus one
                    //Debug.Log("Using " + inventory.Container.Items[i].item.Name + " , you now have" + inventory.Container.Items[i].amount + " left");
                }
            }
            matchstickUI.SetActive(false);          //disable the matchstick UI panel after
        }
        else if (zombieEnemy != null)
        {
            for (int i = 0; i < inventory.Container.Items.Count; i++)                                    //loop through inventory items
            {
                if (inventory.Container.Items[i].item.Name == "Matchstick" && inventory.Container.Items[i].amount > 0)  //find matchstick and check if has more than 0
                {

                    zombieEnemy.Die();                    //call die function of EnemyAI script
                    ScoreManager.instance.addPoint(5);  //add point to score manager
                    GameObject onFire = Instantiate(Fire, target.transform.position, Quaternion.identity);     //instantiate Fire gameobject on collision position
                    Destroy(onFire, 3f);            //destroy Fire gameobject

                    inventory.Container.Items[i].amount = inventory.Container.Items[i].amount - 1;      //minus one
                    //Debug.Log("Using " + inventory.Container.Items[i].item.Name + " , you now have" + inventory.Container.Items[i].amount + " left");
                }
            }
            matchstickUI.SetActive(false);          //disable the matchstick UI panel after
        }
        else if (spiderEnemy != null)
        {
            for (int i = 0; i < inventory.Container.Items.Count; i++)                                    //loop through inventory items
            {
                if (inventory.Container.Items[i].item.Name == "Matchstick" && inventory.Container.Items[i].amount > 0)  //find matchstick and check if has more than 0
                {

                    spiderEnemy.Die();                    //call die function of EnemyAI script
                    ScoreManager.instance.addPoint(5);  //add point to score manager
                    GameObject onFire = Instantiate(Fire, target.transform.position, Quaternion.identity);     //instantiate Fire gameobject on collision position
                    Destroy(onFire, 3f);            //destroy Fire gameobject

                    inventory.Container.Items[i].amount = inventory.Container.Items[i].amount - 1;      //minus one
                    //Debug.Log("Using " + inventory.Container.Items[i].item.Name + " , you now have" + inventory.Container.Items[i].amount + " left");
                }
            }
            matchstickUI.SetActive(false);          //disable the matchstick UI panel after
        }
        else if (skeletonEnemy != null)
        {
            for (int i = 0; i < inventory.Container.Items.Count; i++)                                    //loop through inventory items
            {
                if (inventory.Container.Items[i].item.Name == "Matchstick" && inventory.Container.Items[i].amount > 0)  //find matchstick and check if has more than 0
                {

                    skeletonEnemy.Die();                    //call die function of EnemyAI script
                    ScoreManager.instance.addPoint(5);  //add point to score manager
                    GameObject onFire = Instantiate(Fire, target.transform.position, Quaternion.identity);     //instantiate Fire gameobject on collision position
                    Destroy(onFire, 3f);            //destroy Fire gameobject

                    inventory.Container.Items[i].amount = inventory.Container.Items[i].amount - 1;      //minus one
                    //Debug.Log("Using " + inventory.Container.Items[i].item.Name + " , you now have" + inventory.Container.Items[i].amount + " left");
                }
            }
            matchstickUI.SetActive(false);          //disable the matchstick UI panel after
        }

        //after matchstick use, check if any left in inventory. If none, set bool to false
        for (int i = 0; i < inventory.Container.Items.Count; i++)                                    //loop through inventory items
        {
            if (inventory.Container.Items[i].item.Name == "Matchstick" && inventory.Container.Items[i].amount <= 0)     //check if no more matchsticks
            {
                setHasMatchstick(false);
                Debug.Log("out of matchsticks");
            }
        }
    }

    public void disableMatchstickUI()
    {
        matchstickUI.SetActive(false);
    }

    //used in PlayerInventory.cs so if player collides with enemy and has matchstick in inventory, set target to enemy that player collided with
    public void setTarget(GameObject target)
    {
        this.target = target;
    }

    //called together with setTarget() in PlayerInventory.cs
    public void prompt()
    {
        EnemyAI enemy = target.GetComponent<EnemyAI>();
        if(enemy != null)
        {
            if(enemy.getState() == "BAITED")
            {
                matchstickUI.SetActive(true);
            }
        }
        else
        {
            Destroy(enemy);
        }
        ZombieEnemyAI zombieEnemy = target.GetComponent<ZombieEnemyAI>();
        if (zombieEnemy != null)
        {
            if (zombieEnemy.getState() == "BAITED")
            {
                matchstickUI.SetActive(true);
            }
        }
        else
        {
            Destroy(zombieEnemy);
        }
        SkeletonEnemyAI skeletonEnemy = target.GetComponent<SkeletonEnemyAI>();
        if (skeletonEnemy != null)
        {
            if (skeletonEnemy.getState() == "BAITED")
            {
                matchstickUI.SetActive(true);
            }
        }
        else
        {
            Destroy(skeletonEnemy);
        }
        SpiderEnemyAI spiderEnemy= target.GetComponent<SpiderEnemyAI>();
        if (spiderEnemy != null)
        {
            if (spiderEnemy.getState() == "BAITED")
            {
                matchstickUI.SetActive(true);
            }
        }
        else
        {
            Destroy(spiderEnemy);
        }
    }

    public void setHasMatchstick(bool hasMatchstick)
    {
        this.hasMatchstick = hasMatchstick;
    }

    public bool getHasMatchstick()
    {
        return this.hasMatchstick;
    }
}
