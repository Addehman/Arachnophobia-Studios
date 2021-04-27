using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inventory for give, check or remove item in our inventory. 

public class Inventory : MonoBehaviour
{
    public List<ItemInfo> characterItems = new List<ItemInfo>();
    public ItemDirectory itemDatabase;
    public UIInventory inventoryUI;
    public GameObject toggleInventory;
    bool inventoryIsActive = false;

    private void Start()
    {
        toggleInventory.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {

            if (inventoryIsActive == true)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                toggleInventory.SetActive(false);
                inventoryIsActive = false;
            }

            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                toggleInventory.SetActive(true);
                inventoryIsActive = true;
            }
        }
    }
    public void GiveItem(int id) // itemname
    {
        ItemInfo itemToAdd = itemDatabase.GetItem(id);
        characterItems.Add(itemToAdd);
        inventoryUI.AddNewItem(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.headline);
    }

    public ItemInfo CheckForItems(string title)
    {
        return characterItems.Find(item => item.headline == title);
    }

    public void RemoveItem(string title)
    {
        ItemInfo itemToRemove = CheckForItems(title);
        if (itemToRemove != null)
        {
            characterItems.Remove(itemToRemove);
            inventoryUI.RemoveItem(itemToRemove);
            Debug.Log("Item Removed: " + itemToRemove.headline);
        }
    }
}
