using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Item : Entity
{
    private GameObject _player;

    public Sprite image;
    public bool stackable = false;
    public IItemController itemController;

    [SerializeField] private ItemState _itemState = ItemState.InSpace;
    [SerializeField] private string Description;
    public enum ItemState { InInventory, InSpace }

    void Start()
    {
        base.Start();  
        itemController = GetComponent<IItemController>();
        //itemController.Initialize(_player);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<Player>();
            if (player != null && player._inventoryManager != null)
            {
                if (player._inventoryManager.AddItem(this))
                {
                    gameObject.transform.SetParent(collision.gameObject.transform, false);
                    gameObject.transform.position = Vector3.zero;
                }
            }
        }
    }

}