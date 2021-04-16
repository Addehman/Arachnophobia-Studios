using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTip : MonoBehaviour
{
    private Image itemtip; 

    void Start()
    {
    itemtip = GetComponentInChildren<Image>(); 
    itemtip.gameObject.SetActive(false);
    }

    public void GenerateItemTip(Item item)
    {
    string statText = "";
    if (item.stats.Count > 0)
    {
        foreach (var stat in item.stats)
        {
            statText += stat.Key.ToString() + ": " + stat.Value.ToString() + "\n";
        }
    }
        itemtip.GetComponentInChildren<Text>().text = string.Format("{0}\n{1}\n{2}\n", item.title, item.description, statText);
        // {0} - fix for bold, curse, better format
        itemtip.gameObject.SetActive(true);
    }
    
}

