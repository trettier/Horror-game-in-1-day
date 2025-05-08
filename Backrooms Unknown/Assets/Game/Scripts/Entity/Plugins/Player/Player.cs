using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;

public class Player : Entity
{
    private IPlayerMovementController _playerMovementController;
    private IAnimatorController _playerAnimatorController;
    private ISoundController _soundController;
    private IStaminaController _staminaController;
    private IHideSkill _hideSkill;
    private IRunSkill _RunSkill;
    private IDamageable _damageable;
    public IInventoryManager _inventoryManager;

    private Vector2 _currentDirection = Vector2.down;

    public delegate void DeathEventHandler();
    public event DeathEventHandler OnDeathEvent;

    public delegate void CollectEventHandler();
    public event CollectEventHandler OnCollectEvent;

    private void Start()
    {
        base.Start();
        _playerMovementController = GetComponent<IPlayerMovementController>();
        _playerMovementController.Initialize(rigidbody, speed);
        _playerAnimatorController = GetComponent<IAnimatorController>();
        _playerAnimatorController.Initialize(animator, spriteRenderer, material);
        _soundController = GetComponent<ISoundController>();
        _staminaController = GetComponent<IStaminaController>();
        _hideSkill = GetComponent<IHideSkill>();
        _hideSkill.Initialize(spriteRenderer, collider, rigidbody);
        _RunSkill = GetComponent<IRunSkill>();
        _RunSkill.Initialize(_staminaController, _playerAnimatorController, _soundController, rigidbody, speed);
        _damageable = GetComponent<IDamageable>();
        _damageable.Initialize(maxHealthPoints);
        _damageable.Death += OnDeath;
        _damageable.TakeDamage += OnTakeDamage;
        _inventoryManager = GetComponent<IInventoryManager>();

    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // Найти камеру и сказать ей следовать за этим игроком
        CameraFollowWithLead cameraFollow = FindObjectOfType<CameraFollowWithLead>();
        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        bool isPlayerOccupied = _hideSkill.IsHidden();
        if (isPlayerOccupied)
        {
            _soundController.MakeStepSound(Vector2.zero);
            return;
        }


        _playerMovementController.Move();
        _currentDirection = _playerMovementController.GetCurrentDirection();
        _playerAnimatorController.UpdateAnimation(_currentDirection);
        if (_soundController != null)
        {
            _soundController.MakeStepSound(_currentDirection);
        }
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _hideSkill.TryToHideOrExit();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _inventoryManager.TakeItem();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _inventoryManager.ThrowItem();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _currentDirection = _playerMovementController.GetCurrentDirection();
            _RunSkill.TryToRun(_currentDirection);
        }
        else if (_staminaController.StaminaRegenerationRest() > 0)
        {
            _RunSkill.TryToSlowDown();
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("ObjectToHide"))
        {
            collider.GetComponent<Environment>().SetOutlinedSprite();
            _hideSkill.IsObjectToHideNear(true);
            _hideSkill.SetObjectToHidePosition(collider.transform.position);
        }
        else if (collider.CompareTag("Item"))
        {
            Item item = collider.GetComponent<Item>();
            item.OnPlayerEnterTrigger();
            _inventoryManager.AddItem(item);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("ObjectToHide"))
        {
            collider.GetComponent<Environment>().SetDefaultSprite();
            _hideSkill.IsObjectToHideNear(false);
        }
        else if (collider.CompareTag("Item"))
        {
            collider.GetComponent<Item>().OnPlayerExitTrigger();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            _damageable.ReceiveDamage(maxHealthPoints);
        }
    }

    private void OnDeath()
    {
        OnDeathEvent?.Invoke();
    }

    private void OnTakeDamage()
    {
        _playerAnimatorController.Flash();
        _soundController.MakeHitSound();
    }

    //private IEnumerator DeathWithDelay()
    //{
    //    scream.Play();

    //    yield return new WaitForSeconds(scream.clip.length / 2);

    //    OnDeathEvent?.Invoke();
    //}
}
