using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

public class InventoryManager : MonoBehaviour, IInventoryManager
{
    public static InventoryManager Instance;

    [SerializeField] private InventorySlot[] inventorySlots;
    [SerializeField] private GameObject inventoryItemPrefab;

    //public int maxStackedItems = 4;

    private int selectedSlot = 0;

    private GameObject _player;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _player = gameObject;
        ChangeSelectedSlot(0);
        foreach (var slot in inventorySlots)
        {
            if (slot != null)
            {
                slot.DropItem += RefreshSelectedItem;
            }
        }
    }

    void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 5)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    public void ChangeSelectedSlot(int newValue)
    {
        if (newValue == selectedSlot)
        {
            RefreshSelectedItem();
            return;
        }

        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null && itemInSlot.item != null)
        {
            itemInSlot.DeSelect();
        }

        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].DeSelect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;

        slot = inventorySlots[selectedSlot];
        itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null && itemInSlot.item != null)
        {
            itemInSlot.Select();
        }
    }

    public bool AddItem(Item item)
    {
        //for (int i = 0; i < inventorySlots.Length; i++)
        //{
        //    InventorySlot slot = inventorySlots[i];
        //    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        //    if (itemInSlot != null &&
        //        itemInSlot.item == item &&
        //        itemInSlot.count < maxStackedItems &&
        //        itemInSlot.item.stackable)
        //    {
        //        itemInSlot.count++;
        //        itemInSlot.RefreshCount();
        //        return true;
        //    }
        //}

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null &&
                itemInSlot.item == item)
            {
                return false;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                RefreshSelectedItem();
                return true;
            }
        }
        return false;
    }

    private void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item, _player);
    }

    public Item GetSelectedItem()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null && itemInSlot.item != null)
        {
            return itemInSlot.item;
        }
        return null;
    }

    public void UseSelectedItem()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot.item != null)
        {
            itemInSlot.item.itemController.ActivateItem();
            //itemInSlot.count--;
            //if (itemInSlot.count <= 0)
            //{

            //}
            //else
            //{
            //    itemInSlot.RefreshCount();
            //}
        }
    }

    public void RefreshSelectedItem()
    {
        InventoryItem itemInSlot;
        foreach (var i in inventorySlots)
        {
            itemInSlot = i.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null)
            {
                itemInSlot.DeSelect();
            }
        }

        InventorySlot slot = inventorySlots[selectedSlot];
        itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        slot.Select();
        if (itemInSlot != null && itemInSlot.item != null)
        {
            itemInSlot.item.itemController.TakeItem();
        }
    }
}
