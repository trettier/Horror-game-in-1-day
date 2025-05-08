using UnityEngine;
using UnityEngine.AI;
using System;

public class NavMeshMovementController : MonoBehaviour, IMovementController
{
    [SerializeField] private PatrolPoint _currentPatrolPoint;

    private NavMeshAgent _agent;
    private float _speed;
    private Vector2 _currentPointPosition;
    private Vector2 _movement = Vector2.zero;

    public event Action targetPointAchieved;

    public void Initialize(Rigidbody2D rigidbody, float speed)
    {
        _speed = speed;
        _agent = GetComponent<NavMeshAgent>();

        if (_agent == null)
        {
            Debug.LogError("[NavMeshMovementController] NavMeshAgent не найден на объекте!");
            return;
        }

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = _speed;

        if (_currentPatrolPoint != null)
        {
            _currentPointPosition = _currentPatrolPoint.GetPosition();
            _agent.SetDestination(_currentPointPosition);
        }
    }

    public void Move()
    {
        if (_agent == null || _currentPatrolPoint == null)
        {
            return;
        }

        if (_agent.enabled == false)
        {
            _agent.enabled = true;
            _agent.SetDestination(_currentPointPosition);
        }

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
            {
                _currentPatrolPoint = _currentPatrolPoint.GetRandomPoint();
                _currentPointPosition = _currentPatrolPoint.GetPosition();
                _agent.enabled = false;

                targetPointAchieved.Invoke();
            }
        }

        if (_agent.velocity.sqrMagnitude > 0.01f)
        {
            _movement = _agent.velocity.normalized;
        }
        else
        {
            _movement = Vector2.zero;
        }
    }

    public void SetTargetPosition(Vector2 targetPosition)
    {
        if (_agent != null)
        {
            _agent.enabled = true;
            _currentPointPosition = targetPosition;
            _agent.SetDestination(_currentPointPosition);
        }
    }

    public Vector2 GetCurrentDirection()
    {
        if ( _agent.enabled == false)
        {
            return Vector2.zero;
        }
        return _movement;
    }

    public void StopMotion()
    {
        if (_agent != null)
        {
            _agent.isStopped = true;
        }
    }

    public void SetPatrolPoint(PatrolPoint point)
    {
        _currentPatrolPoint = point;
        if (_agent != null)
        {
            _agent.SetDestination(_currentPatrolPoint.GetPosition());
        }
    }
}
