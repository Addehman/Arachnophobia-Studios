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
	public bool inventoryIsActive = false;

	private void Start()
	{
		toggleInventory.SetActive(false);
	}
	private void Update()
	{
		if (Input.GetButtonDown("Inventory"))
		{
			if (inventoryIsActive == true)
			{
				EnableInventory();
				inventoryIsActive = false;
			}

			else
			{
				DisableInventory();
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

	public void RemoveAllItems()
	{
		for (int i = 0; i < characterItems.Count; i++)
		{
			characterItems.Clear();
			inventoryUI.RemoveItem(null);
		}
	}

	public void EnableInventory()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		toggleInventory.SetActive(false);
	}

	public void DisableInventory()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;
		toggleInventory.SetActive(true);
	}
}
