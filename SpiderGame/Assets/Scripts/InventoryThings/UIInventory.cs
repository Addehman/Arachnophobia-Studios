using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public List<UIItem> uIItems = new List<UIItem>();
    public GameObject slotPrefab;
    public Transform slotPanel;
    public int numberOfSlots = 9;

    private void Awake()
    {
        uIItems = new List<UIItem>();

        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotPanel);
            uIItems.Add(instance.GetComponentInChildren<UIItem>());
        }
    }

    public void UpdateSlot(int slot, Item item)
    {
        uIItems[slot].UpdateItem(item);
    }

    public void AddNewItem(Item item)
    {
        foreach (var uIItem in uIItems)
        {
            if (uIItem.item == null)
            {
                uIItem.UpdateItem(item);
                break;
            }
        }
        //UpdateSlot(uIItems.Where(i => i.item == null), item);
    }

    public void RemoveItem(Item item) 
    {
        foreach (var uIItem in uIItems)
        {
            if (uIItem.item == item)
            {
                uIItem.UpdateItem(item);
                break;
            }
        }
        /*UpdateSlot(uIItems.FindIndex(i => i.item == item), null);*/

    }
}

