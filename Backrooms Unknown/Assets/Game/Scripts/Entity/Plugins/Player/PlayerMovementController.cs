using UnityEngine;

public class PlayerMovementController : MonoBehaviour, IPlayerMovementController
{
    private float _speed;
    private Vector2 _movement;
    private Rigidbody2D _rigidbody;

    public void Initialize(Rigidbody2D rigidbody, float speed)
    {
        _speed = speed;
        _rigidbody = rigidbody;
    }

    public void Move()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        _movement = _movement.normalized;

        _rigidbody.AddForce(_movement * _speed);
    }

    public Vector2 GetCurrentDirection()
    {
        return _movement;
    }
}
