using UnityEngine;
using System.Collections;

public class Behavior : MonoBehaviour, IBehavior
{
    [SerializeField]
    private EnemyState _currentState;
    private IMovementController _movementController;
    private IAnimatorController _animatorController;
    private IFieldOfView _fieldOfView;
    private IInvestigating _investigating;
    private IDamageable _damageable;
    private ISoundController _soundController;
    private Vector2 currentDirection;
    private float elapsedTime;
    private bool _isMoving = true;

    
    public void Initialize(IMovementController movementController,
                         IAnimatorController animatorController,
                         IDamageable damageable,
                         ISoundController soundController)
    {
        _currentState = EnemyState.Patrolling;
        _movementController = movementController;
        _animatorController = animatorController;
        _soundController = soundController;

        // Получаем компоненты
        _fieldOfView = GetComponent<IFieldOfView>();
        _investigating = GetComponent<IInvestigating>();
        _damageable = damageable;

        // Подписываемся на события
        if (_movementController != null)
        {
            _movementController.targetPointAchieved += OnTargetPointAchieved;
        }

        if (_fieldOfView != null)
        {
            _fieldOfView.playerFound += OnPlayerFound;
        }

        if (_investigating != null)
        {
            _investigating.EndOfInvestigation += OnEndOfInvestigation;
        }

        if (_damageable != null)
        {
            _damageable.TakeDamage += OnTakeDamage;
        }
    }

    public void UpdateAction()
    {
        currentDirection = _movementController.GetCurrentDirection();
        switch (_currentState)
        {
            case EnemyState.Spawning:
                break;
            case EnemyState.Patrolling:
                _movementController.Move();
                _animatorController.UpdateAnimation(currentDirection, _isMoving);
                _fieldOfView.DetectTargets(currentDirection);
                _soundController.MakeStepSound(currentDirection);
                break;
            case EnemyState.Chasing:
                _movementController.Move();
                _animatorController.UpdateAnimation(currentDirection, _isMoving);
                _fieldOfView.DetectTargets(currentDirection);
                _soundController.MakeStepSound(currentDirection);
                break;
            case EnemyState.Investigating:
                currentDirection = _investigating.Investigate(currentDirection);
                _animatorController.UpdateAnimation(currentDirection, _isMoving);
                _fieldOfView.DetectTargets(currentDirection);
                _soundController.MakeStepSound(Vector2.zero);
                break;
            case EnemyState.TakingDamage:
                _animatorController.UpdateAnimation(currentDirection, _isMoving);
                break;
            case EnemyState.Dying:
                break;
        }
    }

    private void OnPlayerFound(Vector2 position)
    {
        _movementController.SetTargetPosition(position);
        _currentState = EnemyState.Chasing;
        _isMoving = true;
        _investigating.StopInvestigating();
    }

    private void OnTargetPointAchieved()
    {
        _currentState = EnemyState.Investigating;
        _isMoving = false;
    }

    private void OnEndOfInvestigation()
    {
        _currentState = EnemyState.Patrolling;
        _isMoving = true;
    }

    private void OnTakeDamage()
    {
        StartCoroutine(OnTakeDamageCoroutine());
        _isMoving = false;
    }

    private IEnumerator OnTakeDamageCoroutine()
    {
        _currentState = EnemyState.TakingDamage;
        _animatorController?.Flash();
        _soundController?.MakeHitSound();

        elapsedTime = 0f;
        while (elapsedTime < 0.25f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _currentState = EnemyState.Investigating;
    }

    private void OnDestroy()
    {
        // Отписываемся от событий
        if (_fieldOfView != null)
        {
            _fieldOfView.playerFound -= OnPlayerFound;
        }

        if (_investigating != null)
        {
            _investigating.EndOfInvestigation -= OnEndOfInvestigation;
        }

        if (_damageable != null)
        {
            _damageable.TakeDamage -= OnTakeDamage;
        }
    }
}