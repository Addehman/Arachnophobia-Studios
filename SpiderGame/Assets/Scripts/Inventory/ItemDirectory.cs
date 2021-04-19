using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lists different items, just add as many as possible. Marked with digets or letters.

public class ItemDirectory : MonoBehaviour
{
    public List<ItemInfo> items = new List<ItemInfo>();

    private void Awake()
    {
        BuildItemDirectory();
    }

    // Gets the iteminfo, ask for the id and seperates it with Find. Returns a value.
    public ItemInfo GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    public ItemInfo GetItem(string itemName)
    {
        return items.Find(item => item.headline == itemName);

    }

    void BuildItemDirectory()
    {
        items = new List<ItemInfo>()
        {
            // Creates new Item from ItemInfo descriptions. Ask for key and value, tap in as pleased.
           new ItemInfo(0, "Tomato", "", new Dictionary<string, int>
           {
               {"Flavor", 5},
               {"Vitamin C", 1}
           }),

           new ItemInfo(1, "Banana", "", new Dictionary<string, int>
           {
               {"Flavor", 3},
               {"Potasium", 1},
               {"Bananaflies",2},
               {"Protein",1}
           }),

           new ItemInfo(2, "Blueberry", "", new Dictionary<string, int>
           {
               {"Flavor", 3},
               {"Vitamin C", 1},
               {"Snails",1}
           }),

           new ItemInfo(3, "Apple", "", new Dictionary<string, int>
           {
               {"Flavor", 3},
               {"Vitamin D", 1},
               {"Worms",1}
           })
        };
    }
}
