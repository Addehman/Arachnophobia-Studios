using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTool : MonoBehaviour
{
    private Image itemTool; 

    void Start()
    {
        itemTool = GetComponentInChildren<Image>(); 
        itemTool.gameObject.SetActive(false);
    }

    public void GenerateItemTip(ItemInfo item)
    {
    string statText = "";
    if (item.stats.Count > 0)
    {
        foreach (var stat in item.stats)
        {
            statText += stat.Key.ToString() + ": " + stat.Value.ToString() + "\n";
        }
    }
        itemTool.GetComponentInChildren<Text>().text = string.Format("{0}\n{1}\n{2}\n", item.headline, item.description, statText);
        itemTool.gameObject.SetActive(true);
    }

}

