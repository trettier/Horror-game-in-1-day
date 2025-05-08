using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class InInventoryState : IItemState
{
    private Item _item;

    public InInventoryState(Item item) => _item = item;

    public void Enter()
    {
        _item.gameObject.SetActive(false);
        _item.GetComponent<Rigidbody2D>().simulated = false;
        _item.GetComponent<Collider>().enabled = false;
    }

    public void Exit() { }

    public void Update() { }


    public void OnPlayerEnterTrigger()
    {

    }

    public void OnPlayerExitTrigger()
    {

    }

    public void OnPlayerActivate() { }
}