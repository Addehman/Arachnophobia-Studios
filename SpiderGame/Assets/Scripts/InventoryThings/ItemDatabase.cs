using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        BuildDatabase();
    }

    public Item GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    public Item GetItem(string itemName)
    {
        return items.Find(item => item.title == itemName);

    }

    void BuildDatabase()
    {
        items = new List<Item>()
        {
           new Item(0, "Tomato", "", new Dictionary<string, int>
           {
               {"Flavor", 5},
               {"Vitamin C", 1}
           }),

           new Item(1, "Banana", "", new Dictionary<string, int>
           {
               {"Flavor", 3},
               {"Potasium", 1},
               {"Bananaflies",2},
               {"Protein",1}
           }),

           new Item(2, "Blueberry", "", new Dictionary<string, int>
           {
               {"Flavor", 3},
               {"Vitamin C", 1},
               {"Snails",1}
           }),

           new Item(3, "Apple", "", new Dictionary<string, int>
           {
               {"Flavor", 3},
               {"Vitamin D", 1},
               {"Worms",1}
           })
        };
    }
}
