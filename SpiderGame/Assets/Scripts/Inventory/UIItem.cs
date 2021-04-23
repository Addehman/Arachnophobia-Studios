using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemInfo item;
    private Image spriteImage;
    private UIItem selectedItem;
    private ItemTool itemTip;

    private void Awake()
    {
        spriteImage = GetComponent<Image>();
        UpdateItem(null);
        selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
        itemTip = GameObject.Find("ItemTool").GetComponent<ItemTool>();
    }

    public void UpdateItem(ItemInfo item)
    {
        this.item = item;
        if(this.item != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = this.item.picture;
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
                ItemInfo clone = new ItemInfo(selectedItem.item);
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

