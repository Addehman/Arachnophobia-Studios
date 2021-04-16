using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    private Image spriteImage;
    private UIItem selectedItem;
    private ItemTip itemTip;

    private void Awake()
    {
        spriteImage = GetComponent<Image>();
        UpdateItem(null);
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
        itemTip = GameObject.Find("ItemTip").GetComponent<ItemTip>();
    }

    public void UpdateItem(Item item)
    {
        this.item = item;
        if(this.item != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = this.item.icon;
        }
        else
        {
            spriteImage.color = Color.clear;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.item != null)
        {
            if (selectedItem.item != null)
            {
                Item clone = new Item(selectedItem.item);
                selectedItem.UpdateItem(this.item);
                UpdateItem(clone);
            }

            else
            {
                    selectedItem.UpdateItem(this.item);
                    UpdateItem(null);
            }
        }
                else if (selectedItem.item != null)
                {
                         UpdateItem(selectedItem.item);
                         selectedItem.UpdateItem(null);
                }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.item != null)
        {
            itemTip.GenerateItemTip(this.item);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemTip.gameObject.SetActive(false);
    }
}

