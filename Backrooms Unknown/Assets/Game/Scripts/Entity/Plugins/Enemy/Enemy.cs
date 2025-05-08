using UnityEngine;

public class Enemy : Entity
{
    private IDamageable _damageable;
    private IBehavior _behavior;
    private IMovementController _movementController;
    private IAnimatorController _animatorController;
    private ISoundController _soundController;

    public delegate void DeathEventHandler();
    public event DeathEventHandler OnDeathEvent;

    public PatrolPoint StartPatrolPoint;

    private void Start()
    {
        base.Start();
        _movementController = GetComponent<IMovementController>();
        _movementController.Initialize(rigidbody, speed);
        SetPatrolPoint(StartPatrolPoint);
        _animatorController = GetComponent<IAnimatorController>();
        _animatorController.Initialize(animator, spriteRenderer, material);
        _soundController = GetComponent<ISoundController>();
        _behavior = GetComponent<IBehavior>();
        _behavior.Initialize(_movementController, _animatorController, _damageable, _soundController);



        _damageable = GetComponent<IDamageable>();
        _damageable.Initialize(maxHealthPoints);
        _damageable.Death += Death;
    }
    
    private void FixedUpdate()
    {   
        if (isServer)
        {
            _behavior.UpdateAction();
        }
    }

    private void Death()
    {
        OnDeathEvent?.Invoke();
    }

    public void SetPatrolPoint(PatrolPoint point)
    {
        _movementController.SetPatrolPoint(point);
    }
}
