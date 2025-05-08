using UnityEngine;
using Mirror;

public class NewInventoryManager : NetworkBehaviour, IInventoryManager
{
    [SerializeField] private float speed;

    private Item _itemNear = null;
    //[SyncVar(hook = nameof(OnItemInHandChanged))]
    private NetworkIdentity _itemInHandNetId = null;
    private Item _itemInHand = null;
    private Transform _itemNearTransform = null;
    private Transform _itemInHandTransform = null;

    private bool _isRotatable;
    private float _rotationSpeed;
    private float _offsetDistance;
    private float _followSpeed;

    //private void OnItemInHandChanged(NetworkIdentity oldNetId, NetworkIdentity newNetId)
    //{
    //    if (newNetId != null)
    //    {
    //        _itemInHand = newNetId.GetComponent<Item>();
    //        _itemInHandTransform = _itemInHand.transform;
    //        _itemInHand.SetState(Item.ItemState.InHand);

    //        _isRotatable = _itemInHand.IsRotatable;
    //        _rotationSpeed = _itemInHand.RotationSpeed;
    //        _offsetDistance = _itemInHand.OffsetDistance;
    //        _followSpeed = _itemInHand.FollowSpeed;
    //    }
    //    else
    //    {
    //        _itemInHand = null;
    //        _itemInHandTransform = null;
    //    }
    //}

    public void Initialize()
    {
        _itemInHandTransform = null;
        _isRotatable = false;
        _rotationSpeed = 0;
        _offsetDistance = 0;
        _followSpeed = 0;
    }

    public void ItemNear(Item item)
    {
    }

    public void AddItem(Item item)
    {
        _itemNear = item;
        _itemNearTransform = item.transform;
    }

    public void ThrowItem()
    {
        if (!isLocalPlayer || _itemInHand == null) return;
        var itemNetId = _itemInHand.GetComponent<NetworkIdentity>();
        CmdRemoveItemInHand(itemNetId);
        Vector2 throwDirection = (Vector2)(_itemInHandTransform.position - transform.position).normalized;
        _itemInHand.CmdThrow(speed, throwDirection);
        _itemInHand.SetState(Item.ItemState.InSpace);

        CmdRemoveItemInHand(itemNetId);
        _itemInHandTransform = null;
        _itemInHand = null;
    }

    [Command]
    private void CmdSetItemInHand(NetworkIdentity itemNetId, NetworkConnectionToClient connectionToClientOwner)
    {
        _itemInHandNetId = itemNetId;
        itemNetId.AssignClientAuthority(connectionToClientOwner);
    }

    [Command]
    private void CmdRemoveItemInHand(NetworkIdentity itemNetId)
    {
        _itemInHandNetId = itemNetId;
        itemNetId.RemoveClientAuthority();
    }



    public void TakeItem()
    {
        if (!isLocalPlayer || _itemNear == null) return;

        _itemInHand = _itemNear;
        _itemInHandTransform = _itemNear.transform;

        var itemNetId = _itemNear.GetComponent<NetworkIdentity>();

        _isRotatable = _itemNear.IsRotatable;
        _rotationSpeed = _itemNear.RotationSpeed;
        _offsetDistance = _itemNear.OffsetDistance;
        _followSpeed = _itemNear.FollowSpeed;

        _itemInHandTransform = _itemNear.transform;
        _itemNear.SetState(Item.ItemState.InHand);

        CmdSetItemInHand(itemNetId, connectionToClient);
        _itemNear = null;
        _itemNearTransform = null;
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer || _itemInHandTransform == null) return;

        Rotate();
    }

    private void Rotate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector3 direction = (mousePosition - transform.position).normalized;

        if (_isRotatable)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);
            _itemInHandTransform.rotation = Quaternion.Slerp(_itemInHandTransform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        Vector3 targetPosition = transform.position + direction * _offsetDistance;
        //_itemInHandTransform.position = Vector3.Lerp(_itemInHandTransform.position, targetPosition, _followSpeed * Time.deltaTime);
        Vector3 newPosition = Vector3.Lerp(_itemInHandTransform.position, targetPosition, _followSpeed * Time.deltaTime);
        _itemInHand.UpdatePosition(newPosition);
        //_itemInHand.GetComponent<Rigidbody2D>().MovePosition(targetPosition);
    }
}