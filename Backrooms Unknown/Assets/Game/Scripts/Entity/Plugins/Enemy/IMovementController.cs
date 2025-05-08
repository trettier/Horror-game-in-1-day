using UnityEngine;
using System;

public interface IMovementController
{

    public event Action targetPointAchieved;

    void Initialize(Rigidbody2D rigidbody, float speed);

    void Move();

    void SetTargetPosition(Vector2 targetPosition);

    void SetPatrolPoint(PatrolPoint point);

    Vector2 GetCurrentDirection();
}