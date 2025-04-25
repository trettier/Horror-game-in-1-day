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
        _soundController.MakeStepSound(_currentDirection);
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
            _inventoryManager.UseSelectedItem();
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
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("ObjectToHide"))
        {
            collider.GetComponent<Environment>().SetDefaultSprite();
            _hideSkill.IsObjectToHideNear(false);
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
