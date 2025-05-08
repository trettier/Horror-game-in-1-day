using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class InHandState : IItemState
{
    private Item _item;
    private Transform _transform;
    private bool _isRotatable;
    private float _rotationSpeed;
    private float _offsetDistance;
    private float _followSpeed;

    public InHandState(Item item) => _item = item;

    public void Enter()
    {
        _transform = _item.gameObject.transform;
        _isRotatable = _item.IsRotatable;
        _rotationSpeed = _item.RotationSpeed;
        _offsetDistance = _item.OffsetDistance;
        _followSpeed = _item.FollowSpeed;
        _item.itemAnimatorController.OutlineOff();
        _item._activateItem.Activate();

    }

    public void Exit() 
    {
        _item._activateItem.Deactivate();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _item.SetState(Item.ItemState.InSpace); // Выбросить предмет
        }
        else if (Input.GetMouseButtonDown(0))
        {
            //_item.itemController.Use(); // Использовать предмет
        }
    }

    public void OnPlayerEnterTrigger()
    {
    }

    public void OnPlayerExitTrigger()
    {

    }

    public void OnPlayerActivate() 
    {
        _item._activateItem.Activate();
    }
}