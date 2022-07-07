using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : MonoBehaviour
{
    public GameObject noKeyTutorial;
    public GameObject hasKeyTutorial;
    public InventoryObject inventory;       //to access player inventory and check for key
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")                  //check that collided with player   
        {
            //loop through inventory items and check if player has key
            for (int i = 0; i < inventory.Container.Items.Count; i++)
            {
                if (inventory.Container.Items[i].item.Name == "Key" && inventory.Container.Items[i].amount > 0)
                {
                    hasKeyTutorial.gameObject.SetActive(true);      //display success tutorial
                    player.transform.position = new Vector2(-6, -17);
                    return;                                         //return so noKeyTutorial does not activate
                }
            }
            noKeyTutorial.gameObject.SetActive(true);       //display fail tutorial
        }
    }

    //on exit collider, disable the tutorials
    private void OnTriggerExit2D(Collider2D collision)
    {
        hasKeyTutorial.gameObject.SetActive(false);
        noKeyTutorial.gameObject.SetActive(false);
    }
}
