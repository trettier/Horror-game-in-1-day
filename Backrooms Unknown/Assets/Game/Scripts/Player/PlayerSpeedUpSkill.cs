using UnityEngine;

public class PlayerSpeedUpSkill : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator animator;
    [SerializeField] private float animationSpeed = 1.5f;
    [SerializeField] private float speedCoefficent = 0.2f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void TryToSpeedUp(Vector2 movement, float speed)
    {
        _rb.AddForce(movement * speed * speedCoefficent);
        animator.speed = 1.5f;
    }
    public void Slowdown()
    {
        animator.speed = 1f;
    }
}
