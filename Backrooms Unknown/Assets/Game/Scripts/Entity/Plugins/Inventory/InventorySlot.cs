using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    //public Color selectedColor, notSelectedColor;

    public event Action DropItem; 

    void Awake()
    {
        DeSelect();
    }
    public void Select()
    {
        transform.localScale = Vector3.one * 1.25f;
    }

    public void DeSelect()
    {
        transform.localScale = Vector3.one;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }

    public void AfterDrop()
    {
        DropItem.Invoke();
    }
}
