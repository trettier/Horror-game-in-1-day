using UnityEngine;
using System;

public class StandardMovementController : MonoBehaviour, IMovementController
{

    [SerializeField] private PatrolPoint _currentPatrolPoint;

    private float _speed;
    private Vector2 _movement;
    private Vector2 _currentPointPosition;
    private Rigidbody2D _rigidbody;

    public event Action targetPointAchieved;

    public void Initialize(Rigidbody2D rigidbody, float speed)
    {
        _speed = speed;
        _rigidbody = rigidbody;
        _currentPointPosition = _currentPatrolPoint.GetPosition();
    }

    public void Move()
    {
        if (Vector2.Distance((Vector2)transform.position, _currentPointPosition) < 0.1f)
        {
            _currentPatrolPoint = _currentPatrolPoint.GetRandomPoint();
            _currentPointPosition = _currentPatrolPoint.GetPosition();
            targetPointAchieved.Invoke();
            return;
        }
        _movement = (_currentPointPosition - (Vector2)transform.position).normalized;
        _rigidbody.AddForce(_movement * _speed);
    }

    public void SetTargetPosition(Vector2 targetPosition)
    {
        _currentPointPosition = targetPosition;
    }

    public Vector2 GetCurrentDirection()
    {
        return _movement;
    }

    public void StopMotion()
    {

    }
}