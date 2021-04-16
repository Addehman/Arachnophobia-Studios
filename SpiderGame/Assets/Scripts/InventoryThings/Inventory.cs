using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> characterItems = new List<Item>();
    public ItemDatabase itemDatabase;
    public UIInventory inventoryUI;
    public GameObject toggleInventory;
    bool inventoryIsActive = false;

    private void Start()
    {
        toggleInventory.SetActive(false);

        /*GiveItem("Banana");
        GiveItem("Blueberry");
        GiveItem("Apple");
        GiveItem("Tomato");
        GiveItem("Apple");
        GiveItem("Apple");*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            /*inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);*/
        
            if (inventoryIsActive == true)
            {
                toggleInventory.SetActive(false);
                inventoryIsActive = false;
            }

            else
            {
                toggleInventory.SetActive(true);
                inventoryIsActive = true;
            }
        }
    }

    public void GiveItem(int id) // itemname
    {
        Item itemToAdd = itemDatabase.GetItem(id);
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.title);
    }

    public Item CheckForItems(string title )
    {
        return characterItems.Find(item => item.title == title);
    }

    public void RemoveItem(string title)
    {
        Item itemToRemove = CheckForItems(title);
        if (itemToRemove != null)
        {
            characterItems.Remove(itemToRemove);
            inventoryUI.RemoveItem(itemToRemove);
            Debug.Log("Item Removed: " + itemToRemove.title);
        }
    }
}
