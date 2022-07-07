using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject inventoryPrefab;  //1 single prefab that every item in our inventory uses

    public InventoryObject inventory;   //can link directly to player inventory in editor because Scriptable Object

    //start location for inventory slots
    public int x_start;
    public int y_start;

    //formatting of individual item slots
    public int space_between_items_x;
    public int number_of_columns;
    public int space_between_items_y;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();     //key: InventorySlot, value: GameObject

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        //loop through items in our inventory
        for (int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];

            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);    //set position, rotation and parent
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].uiDisplay;   //access image of inventory prefab so we can change sprite display
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");          //edit TextMeshPro text
                                                                                                                        //n0 to format in commas

            //add item to our itemsDisplayed dictionary
            itemsDisplayed.Add(slot, obj);     //detectBait is the object we just created
        }
    }

    public void UpdateDisplay()
    {
        //loop through items in our inventory
        for (int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];

            if (itemsDisplayed.ContainsKey(slot))     //if item is already in our inventory...
            {
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");          //edit TextMeshPro text
                                                                                                                                                               //n0 to format in commas
            }
            else        //else if item is NOT already in our inventory...
            {
                //similar to CreateDisplay()
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);    //set position, rotation and parent
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.Id].uiDisplay;   //access image of inventory prefab so we can change sprite display
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");          //edit TextMeshPro text

                //add item to our itemsDisplayed dictionary
                itemsDisplayed.Add(slot, obj);     //detectBait is the object we just created
            }
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(x_start + (space_between_items_x * (i % number_of_columns)), y_start + (-space_between_items_y * (i / number_of_columns)), 0f);
    }

}
