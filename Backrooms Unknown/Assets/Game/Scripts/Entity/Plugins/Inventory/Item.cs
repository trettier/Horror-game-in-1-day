using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class Item : Entity
{
    public enum ItemState { InInventory, InSpace, InHand }

    [SerializeField]
    [SyncVar(hook = nameof(OnStateChanged))] // Синхронизация по сети
    private ItemState _currentState = ItemState.InSpace;

    private IItemState _currentStateHandler;
    private Dictionary<ItemState, IItemState> _states;

    public IItemController itemController;
    public ItemAnimatorController itemAnimatorController;

    public bool IsRotatable = false;
    public float RotationSpeed;
    public float OffsetDistance;
    public float FollowSpeed;

    public bool _isPlayerNear = false;

    public IActivateItem _activateItem;

    void Start()
    {
        base.Start();
        itemController = GetComponent<IItemController>();
        itemAnimatorController = GetComponent<ItemAnimatorController>();
        itemAnimatorController.Initialize(material);

        // Инициализация состояний
        _states = new Dictionary<ItemState, IItemState>
        {
            { ItemState.InInventory, new InInventoryState(this) },
            { ItemState.InSpace, new InSpaceState(this) },
            { ItemState.InHand, new InHandState(this) }
        };

        OnStateChanged(_currentState, _currentState);

        _activateItem = GetComponent<IActivateItem>();
    }

    void FixedUpdate()
    {
        _currentStateHandler?.Update();
    }
    public void UpdatePosition(Vector3 newPosition, NetworkConnectionToClient sender = null)
    {
        transform.position = newPosition;
    }

    [Command(requiresAuthority = false)]
    public void CmdThrow(float speed, Vector2 direction, NetworkConnectionToClient sender = null)
    {
        rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
    }


    void OnEnable()
    {
        SetState(_currentState);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    _currentStateHandler?.HandleTriggerEnter(collision, spriteRenderer.material);
    //}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
    }

    // Сеттер состояния (вызывается локально и синхронизируется)
    [Command(requiresAuthority = false)]
    public void SetState(ItemState newState)
    {
        _currentState = newState;
    }

    // Вызывается при синхронизации состояния
    private void OnStateChanged(ItemState oldState, ItemState newState)
    {
        _currentStateHandler?.Exit();
        _currentStateHandler = _states[newState];
        _currentStateHandler.Enter();
    }

    public void OnPlayerEnterTrigger()
    {
        _currentStateHandler.OnPlayerEnterTrigger();
    }

    public void OnPlayerExitTrigger()
    {
        _currentStateHandler.OnPlayerExitTrigger();
    }
}