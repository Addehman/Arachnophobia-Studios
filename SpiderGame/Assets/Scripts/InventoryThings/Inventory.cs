using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> characterItems = new List<Item>();
    public ItemDatabase itemDatabase;

    private void Start()
    {
        GiveItem(0);
        GiveItem(1);
        GiveItem(2);
        GiveItem(3);
        RemoveItem(1);
    }

    public void GiveItem(int id)
    {
        Item itemToAdd = itemDatabase.GetItem(id);
        characterItems.Add(itemToAdd);
        Debug.Log("Added item: " + itemToAdd.title);
    }

    public Item CheckForItems(int id)
    {
        return characterItems.Find(item => item.id == id);
    }

    public void RemoveItem(int id)
    {
        Item item = CheckForItems(id);
        if (item != null)
        {
            characterItems.Remove(item);
            Debug.Log("Item Removed: " + item.title);
        }
    }
}
