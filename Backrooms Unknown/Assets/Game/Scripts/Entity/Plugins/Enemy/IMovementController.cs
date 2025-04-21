using UnityEngine;
using System;

public interface IMovementController
{

    public event Action targetPointAchieved;

    void Initialize(Rigidbody2D rigidbody, float speed);

    void Move();

    void SetTargetPosition(Vector2 targetPosition);

    Vector2 GetCurrentDirection();
}