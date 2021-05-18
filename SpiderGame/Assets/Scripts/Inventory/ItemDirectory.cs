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
           }),

            new ItemInfo(4, "Carrot", "", new Dictionary<string, int>
           {
               {"Flavor", 1},
               {"Vitamin C", 1},
               {"Dirt",-1}
           }),

             new ItemInfo(5, "Potato", "", new Dictionary<string, int>
           {
               {"Flavor", 3},
               {"Vitamin D", 2},
               {"CowPoop",10}
           }),

              new ItemInfo(6, "Cheese", "", new Dictionary<string, int>
           {
               {"Flavor", 3},
               {"Vitamin C", 1},
               {"French nails",2}
           }),

              new ItemInfo(7, "Tbone", "", new Dictionary<string, int>
           {
               {"Protein", 10},
               {"Vitamin B12", 6},
               {"Iron",50}
           }),

              new ItemInfo(8, "ChickenBone", "", new Dictionary<string, int>
           {
               {"te", 32},
               {"de D", 21},
               {"gre",101}
           }),

              new ItemInfo(9, "Cookie", "", new Dictionary<string, int>
           {
               {"Sugar", 301},
               {"Chocolate", 10},
               {"Diabetes",9000}
           }),
        };
    }
}
