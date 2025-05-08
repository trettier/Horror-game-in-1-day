using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class InSpaceState : IItemState
{
    private Item _item;

    public InSpaceState(Item item) => _item = item;
    private bool _isPlayerNear = false;

    public void Enter()
    {
        _item.gameObject.SetActive(true);
        _item.gameObject.tag = "Item";
        //_item.GetComponent<Rigidbody2D>().simulated = true;
    }

    public void Exit() 
    { 
        _isPlayerNear = false;
        _item.gameObject.tag = "ItemInHand";
    }

    public void Update() 
    {
        if (!_isPlayerNear)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _item.SetState(Item.ItemState.InHand);
        }
    }

    public void OnPlayerEnterTrigger()
    {
        _item.itemAnimatorController.OutlineOn();
        _isPlayerNear = false;
    }

    public void OnPlayerExitTrigger()
    {
        _item.itemAnimatorController.OutlineOff();
        _isPlayerNear = true;
    }

    public void OnPlayerActivate() { }
}