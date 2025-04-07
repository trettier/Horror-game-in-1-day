using UnityEngine;

public interface IPlayerMovementController
{
    void Initialize(Rigidbody2D rigidbody, float speed);
    void Move();

    Vector2 GetCurrentDirection();
}