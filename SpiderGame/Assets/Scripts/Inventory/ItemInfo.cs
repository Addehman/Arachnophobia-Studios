using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script makes it possible to create numerous items with information in them.

public class ItemInfo
{
    public int id;
    public string headline;
    public string description;
    public Sprite picture;
    public Dictionary<string, int> stats = new Dictionary<string, int>();

    // Creates a function that makes a constructer
    public ItemInfo(int id, string headline, string description, Dictionary<string, int> stats)
    {
        this.id = id;
        this.headline = headline;
        this.description = description;
        this.picture = Resources.Load<Sprite>("Sprites/Refrigerator/" + headline);
        this.stats = stats;
    }

    //Copies one item and create another. This is used so we can move items around the inventory. Nested - copymachine :) 
    public ItemInfo(ItemInfo ItemInfo)
    {
        this.id = ItemInfo.id;
        this.headline = ItemInfo.headline;
        this.description = ItemInfo.description;
        this.picture = Resources.Load<Sprite>("Sprites/Refrigerator/" + ItemInfo.headline);
        this.stats = ItemInfo.stats;
    }

}
