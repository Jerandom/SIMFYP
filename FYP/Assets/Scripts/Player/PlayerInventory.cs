using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public MatchstickUI matchstick;     //for working with MatchstickUI.cs
    private bool isColliding = false;
    
    //public GameObject pauseMenu;      //shifted pause functionalities to PauseMenu.cs

    //for collision trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Because capsule collider has more contact points, use boolean to prevent multiple calls of same collision.
        if (isColliding)
            return;
        isColliding = true;     //next call will return 

        var item = collision.GetComponent<GroundItem>();
        if (item)   //if collided with item
        {
            if (!item.getThrownStatus())    //if isThrown is false... (so that player cannot pickup an object that is currently being thrown)
            {
                if(item.item.Name == "Matchstick")
                {
                    matchstick.setHasMatchstick(true);
                }

                inventory.AddItem(new Item(item.item), 1);    //add 1 of the item to inventory
                SaveSystem.Instance.groundItemList.Remove(collision.GetComponent<GroundItem>());
                Destroy(collision.gameObject);                //destroy the picked up item
                //Debug.Log("You picked up: " + item.item.Name + "  ID: " + item.item.Id + " Thrown Status: " + item.getThrownStatus());
            }
        }

        if (collision.tag == "Enemy")
        {
            if (matchstick.getHasMatchstick() == true)
            {
                matchstick.setTarget(collision.gameObject);
                matchstick.prompt();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Exiting matchstick range");
            matchstick.disableMatchstickUI();       //once player collider exits enemy, disable the matchstick UI
        }
    }

    private void Start()
    {
        inventory.StartInventory();
        matchstick = GetComponent<MatchstickUI>();
    }

    private void Update()
    {
        //every frame, set back to false so OnTriggerEnter2D code can run if collision detected
        isColliding = false;

        /*
        //when user presses escape, open pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
        }
        */

        //when user presses space
        if (Input.GetKeyDown("o"))      
        {
            inventory.Save();
        }

        //when user presses space
        if (Input.GetKeyDown("p"))      
        {
            inventory.Load();
        }
    }

    //clear items in inventory
    private void OnApplicationQuit()
    {
        inventory.Container.Items.Clear();
    }
}
