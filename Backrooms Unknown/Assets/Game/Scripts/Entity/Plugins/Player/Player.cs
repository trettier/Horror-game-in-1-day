using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Entity
{
    private IPlayerMovementController _playerMovementController;
    private IAnimatorController _playerAnimatorController;
    private ISoundController _soundController;
    public IStaminaController _staminaController;
    public IHideSkill _hideSkill;
    public IRunSkill _RunSkill;
    public IDamageable _damageable;

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
        _damageable.Initialize(_playerAnimatorController, _soundController, maxHealthPoints);
        _damageable.OnDeath += Death;

    }

    private void FixedUpdate()
    {
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
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _hideSkill.TryToHideOrExit();
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
        //if (collider.CompareTag("Mirror"))
        //{
        //    collider.GetComponent<Environment>().SetOutlinedSprite();
        //    playerNearMirror = true;
        //}
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("ObjectToHide"))
        {
            collider.GetComponent<Environment>().SetDefaultSprite();
            _hideSkill.IsObjectToHideNear(false);
        }
        //if (collider.CompareTag("Mirror"))
        //{
        //    collider.GetComponent<Environment>().SetDefaultSprite();
        //    playerNearMirror = false;
        //}
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            _damageable.RecieveDamage(maxHealthPoints);
        }
    }

    private void Death()
    {
        OnDeathEvent?.Invoke();
    }

    //private IEnumerator DeathWithDelay()
    //{
    //    scream.Play();

    //    yield return new WaitForSeconds(scream.clip.length / 2);

    //    OnDeathEvent?.Invoke();
    //}
}
