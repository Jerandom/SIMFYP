using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    public InventoryObject inventory;
    public Transform playerTransform;
    public Transform throwPoint;
    private float force = 0.5f;
    public int numberPressed = 0;

    public Identification[] prefabArray;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (!PauseMenu.paused)      //check that game is not paused (so cant throw objects while paused)
            {
                if (Input.GetKeyDown("1") || Input.GetKeyDown("2") || Input.GetKeyDown("3") || Input.GetKeyDown("4") || Input.GetKeyDown("5") || Input.GetKeyDown("6") || Input.GetKeyDown("7"))
                {
                    numberPressed = int.Parse(Input.inputString) - 1;
                    //Debug.Log(numberPressed);   //to access correct inventory item

                    if (inventory.Container.Items[numberPressed].amount > 0)    //if inventory not empty
                    {
                        StartCoroutine(ThrowItem(numberPressed, inventory.Container.Items[numberPressed].item.Name));   //Start Coroutine for using the item
                    }
                }
            }
        }
        catch (System.ArgumentOutOfRangeException e)    //when inventory is empty and user presses key
        {
            Debug.Log(e);
        }
    }
    
    IEnumerator ThrowItem(int index, string itemName)
    {
        //Remove one count of item from inventory slot
        //inventory.Container.Items[index].amount = inventory.Container.Items[index].amount - 1;
        //Debug.Log("Dropping an item, you now have: " + inventory.Container.Items[0].item.Name + " x " + inventory.Container.Items[0].amount);

        
        if (itemName != "Matchstick" && itemName != "Key")      //if item is not matchstick/key, allow for throwing...
        {
            //Instantiate and launch prefab object
            GameObject thrownItem;
            for (int i = 0; i < prefabArray.Length; i++)
            {
                if (prefabArray[i].id == itemName)
                {
                    thrownItem = Instantiate(prefabArray[i].prefab, throwPoint.position, playerTransform.rotation);
                    inventory.Container.Items[index].amount = inventory.Container.Items[index].amount - 1;

                    if (itemName == "Molotov" || itemName == "HolyWater")      //throw faster if molotov as compared to bait
                    {
                        force = 1f;
                    }
                    else
                    {
                        force = 0.5f;
                    }

                    //Add force to thrownItem rigidbody and setThrownStatus from GroundItem.cs to true (object is being thrown)
                    Rigidbody2D rb = thrownItem.GetComponent<Rigidbody2D>();
                    rb.AddForce(throwPoint.up * force, ForceMode2D.Impulse);
                    thrownItem.GetComponent<GroundItem>().setThrownStatus(true);

                    //wait for 1 second, then setThrownStatus back to false (object is not being thrown, can be picked up/interacted with)
                    yield return new WaitForSeconds(1);
                    if (thrownItem.gameObject != null)
                    {
                        thrownItem.GetComponent<GroundItem>().setThrownStatus(false);
                    }
                }
            }

            //thrownItem.GetComponent<Collider2D>().enabled = false;
            //thrownItem.GetComponent<Collider2D>().enabled = true;
            //Debug.Log(thrownItem.GetComponent<GroundItem>().getThrownStatus());
            
        }
    }
    
    public void addForce(GameObject thrownItem)
    {
        //Add force to thrownItem rigidbody and setThrownStatus from GroundItem.cs to true (object is being thrown)
        Rigidbody2D rb = thrownItem.GetComponent<Rigidbody2D>();
        rb.AddForce(throwPoint.up * force, ForceMode2D.Impulse);
        thrownItem.GetComponent<GroundItem>().setThrownStatus(true);

        if (thrownItem.gameObject != null)
        {
            thrownItem.GetComponent<GroundItem>().setThrownStatus(false);
        }
    }

    /*
    IEnumerator ThrowBait(GameObject thrownItem)
    {
        Rigidbody2D rb = thrownItem.GetComponent<Rigidbody2D>();
        thrownItem.GetComponent<Collider2D>().enabled = false;
        rb.AddForce(throwPoint.up * force, ForceMode2D.Impulse);

        //wait for 1 second, then re-enable the circle collider and apply reverse force to stop object
        yield return new WaitForSeconds(1);
        thrownItem.GetComponent<Collider2D>().enabled = true;
    }
    */
    /*
    IEnumerator ThrowMeat(int index)
    {
        //Remove one count of item from inventory slot
        inventory.Container.Items[index].amount = inventory.Container.Items[index].amount - 1;
        //Debug.Log("Dropping an item, you now have: " + inventory.Container.Items[0].item.Name + " x " + inventory.Container.Items[0].amount);

        //Instantiate and launch prefab object
        GameObject meat = Instantiate(meatPrefab, throwPoint.position, playerTransform.rotation);
        Rigidbody2D rb = meat.GetComponent<Rigidbody2D>();
        meat.GetComponent<Collider2D>().enabled = false;
        rb.AddForce(throwPoint.up * force, ForceMode2D.Impulse);

        //wait for 1 second, then re-enable the circle collider and apply reverse force to stop object
        yield return new WaitForSeconds(1);
        meat.GetComponent<Collider2D>().enabled = true;
        rb.AddForce(throwPoint.up * reverseForce, ForceMode2D.Impulse);
    }
    */
}


[System.Serializable]
public class Identification     //class to identify prefab in prefabArray
{
    public string id;   //matching prefab name
    public GameObject prefab;

}
