using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler,IDragHandler,IBeginDragHandler,IEndDragHandler, IPointerClickHandler
{
    [Header("Inventory Detail")] 
    public Inventory inventory;

    [Header("Slot Detail")] 
    public SO_ITEM item;
    public int stack;

    [Header("UI")] 
    public Color emthyColor;
    public Color itemColor;
    public Image icon;
    public TextMeshProUGUI stackText;

    [Header("Drag and Drop")] 
    public int siblingIndex;
    public RectTransform draggable;
    public Canvas canvas;
    private CanvasGroup canvasGroup;
    

    void Start()
    {
        
        canvasGroup = GetComponent<CanvasGroup>();
        siblingIndex = transform.GetSiblingIndex();
        
    }
    
    public void SetThisSlot(SO_ITEM newItem, int amount)
    {
        item = newItem;
        if (icon != null && newItem != null && newItem.icon != null) 
        {
            icon.sprite = newItem.icon;
        }

        int ItemAmount = amount;

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0, newItem.maxStack);
        stack = intInthisSlot;

        CheckShowText();
        
        int amountLeft = ItemAmount - intInthisSlot;
        if (amountLeft > 0)
        {
            InventorySlot slot = inventory.isEmpthyslotleft(newItem,this);
            
            if (slot == null)
            {
                // Drop Item
                return;
            }
            else
            {
                slot.SetThisSlot(newItem, amountLeft);
            }
        }
    }

    public void CheckShowText()
    {
        
        stackText.text = stack.ToString();
        if (item.maxStack < 2)
        {
            stackText.gameObject.SetActive(false);
        }
        else
        {
            if (stack > 1)
                stackText.gameObject.SetActive(true);
            else
                stackText.gameObject.SetActive(false);
        }
    }

    #region Drag and Drop Method
    
    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot slot = eventData.pointerDrag.GetComponent<InventorySlot>();
        if (slot != null)
        {
            if (slot.item = item)
            {
                // Merge
                MergeThisSlot(slot);
            }
            else
            {
                // Swap
                SwapSlot(slot);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        draggable.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();
        inventory.SetLayoutControlChild(false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        draggable.anchoredPosition = Vector2.zero;
        transform.SetSiblingIndex(siblingIndex);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item == inventory.EMPTHY_ITEM)
                return;
            // Inventory open mini canvas
            inventory.OpenMiniCanvas(eventData.position);
            inventory.SetRightClickSlot(this);
        }
    }

    public void SwapSlot(InventorySlot newSlot)
    {
        SO_ITEM keepItem;
        int keepstack;

        keepItem = item;
        keepstack = stack;
        
        SetSwap(newSlot.item,newSlot.stack);
        
        newSlot.SetSwap(keepItem,keepstack);
    }

    public void SetSwap(SO_ITEM swapItem, int amount)
    {
        item = swapItem;
        stack = amount;
        icon.sprite = swapItem.icon;
        
        CheckShowText();
    }
    

    public void MergeThisSlot(InventorySlot mergeslot)
    {
        if (stack == item.maxStack || mergeslot.stack == mergeslot.item.maxStack)
        {
            SwapSlot(mergeslot);
            return;
        }
        int ItemAmount = stack + mergeslot.stack;

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0,item.maxStack);
        stack = intInthisSlot;

        CheckShowText();
        
        int amountLeft = ItemAmount - intInthisSlot;
        if (amountLeft > 0)
        {
            // Set slot
            mergeslot.SetThisSlot(mergeslot.item, amountLeft);
            
        }
        else
        {
            // Remove
            inventory.RemoveItem(mergeslot);
            
        }
    }
    public void MergeThisSlot(SO_ITEM mergeItem,int mergeAmount)
    {
        item = mergeItem;
        icon.sprite = mergeItem.icon;

        int ItemAmount = stack + mergeAmount;

        int intInthisSlot = Mathf.Clamp(ItemAmount, 0,item.maxStack);
        stack = intInthisSlot;

        CheckShowText();
        
        int amountLeft = ItemAmount - intInthisSlot;
        if (amountLeft > 0)
        {
            InventorySlot slot = inventory.isEmpthyslotleft(mergeItem, this);
            if (slot == null)
            {
                inventory.DropItem(mergeItem,amountLeft);
                return;
            }
            else
            {
                slot.MergeThisSlot(mergeItem,amountLeft);
            }
        }
    }


    public void UseItem()
    {
        stack = Mathf.Clamp(stack - 1, 0, item.maxStack);
        if (stack > 0)
        {
            CheckShowText();
        }
        else
        {
            inventory.RemoveItem(this);
        }
        
    }
    
    #endregion
}
