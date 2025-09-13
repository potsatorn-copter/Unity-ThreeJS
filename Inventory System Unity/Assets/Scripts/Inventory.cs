using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [Header("Inventory")] 
    public SO_ITEM EMPTHY_ITEM;

    public Transform slotPrefab;
    public Transform inventoryPanel;
    protected GridLayoutGroup gridLayoutGroup;
    [Space(5)] 
    public int slotAmount = 30;
    public InventorySlot[] inventoryslots;

    [Header("mini canvas")] 
    public RectTransform minicanvas;
    [SerializeField] protected InventorySlot rightClickslot;
    
    // Start is called before the first frame update
    void Start()
    {
        gridLayoutGroup = inventoryPanel.GetComponent<GridLayoutGroup>();
        CreateInventorySlots();
    }
    
    
    #region Invetory Methods

    public void AddItem(SO_ITEM item, int amount)
    {
        InventorySlot slot = isEmpthyslotleft(item);
        {
            if (slot == null)
            {
                // Full
                DropItem(item,amount);
                return;
            }
            slot.MergeThisSlot(item,amount);
            
        }
    }
    public void UseItem() // Onclick event
    {
        // use
        rightClickslot.UseItem();
        OnFinishMiniCanvas();
    }

    public void DropItem()
    {
       ItemSpawner.Instance.SpawnItem(rightClickslot.item,rightClickslot.stack);
        DestroyItem();
    }

    public void DropItem(SO_ITEM item, int amount)
    {
        ItemSpawner.Instance.SpawnItem(item,amount);
    }

    public void DestroyItem()
    {
        rightClickslot.SetThisSlot(EMPTHY_ITEM,0);
        OnFinishMiniCanvas();
    }
    public void RemoveItem(InventorySlot slot)
    {
        slot.SetThisSlot(EMPTHY_ITEM, 0);
    }

    public void SortItem(bool Ascending = true)
    {
        SetLayoutControlChild(true);

        List<InventorySlot> slotHaveItem = new List<InventorySlot>();
        foreach (InventorySlot slot in inventoryslots)
        {
            if (slot.item != EMPTHY_ITEM)
            {
                slotHaveItem.Add(slot);
            }
        }

        var sortArray = Ascending ? 
            slotHaveItem.OrderBy(slot => slot.item.id).ToArray() : 
            slotHaveItem.OrderByDescending(slot => slot.item.id).ToArray();
        foreach (InventorySlot slot in inventoryslots)
        {
            Destroy(slot.gameObject);
        }
        CreateInventorySlots();
        foreach (InventorySlot slot in sortArray)
        {
            AddItem(slot.item, slot.stack);
        }
    }
    public void CreateInventorySlots()
    {
        inventoryslots = new InventorySlot[slotAmount];
        for (int i = 0; i < slotAmount; i++)
        {
            Transform slot = Instantiate(slotPrefab, inventoryPanel);
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();

            inventoryslots [i] = inventorySlot;
            inventorySlot.inventory = this;
            inventorySlot.SetThisSlot(EMPTHY_ITEM,0);
        }
    }

    public InventorySlot isEmpthyslotleft(SO_ITEM itemChecker = null, InventorySlot itemSlot = null)
    {
        InventorySlot firstEmthySlot = null;
        foreach (InventorySlot slot in inventoryslots)
        {
            if (slot == itemSlot)
                continue;


            if (slot.item == itemChecker)
            {
                if (slot.stack < slot.item.maxStack)
                {
                    return slot;
                }
            }
            else if (slot.item == EMPTHY_ITEM && firstEmthySlot == null)
                firstEmthySlot = slot;
        }
        return firstEmthySlot;
    }

    public void SetLayoutControlChild(bool isControlled)
    {
        gridLayoutGroup.enabled = isControlled;
    }

    #endregion
    
    #region Mini Canvas

    public void SetRightClickSlot(InventorySlot slot)
    {
        rightClickslot = slot;
    }

    public void OpenMiniCanvas(Vector2 clickPosition)
    {
        minicanvas.position = clickPosition;
        minicanvas.gameObject.SetActive(true);
    }

    public void OnFinishMiniCanvas()
    {
        rightClickslot = null;
        minicanvas.gameObject.SetActive(false);
    }
    #endregion
    

}
