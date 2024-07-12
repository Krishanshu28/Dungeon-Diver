using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;


    public int quantity;

    public Sprite sprite;

    [TextArea]  //inspector pe direct likh lena
    public string itemDescription;

    private InventoryManager inventoryManager;

    bool itemPickUp;
    
    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    //for picking up item from "F" key and to add from gamepad 
    void OnItemPickUp()
    {
        itemPickUp = true;
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player") && itemPickUp)
        {
            //print("pick");
            int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription);
            if(leftOverItems <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                quantity = leftOverItems;
            }
            print("accept");
            itemPickUp =false;
        }
    }
    
}
