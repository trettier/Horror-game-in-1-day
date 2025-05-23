using UnityEngine;
using System.Collections.Generic;
using Mirror;

public abstract class Entity : NetworkBehaviour
{
    [Header("Entity params")]
    [SerializeField] protected float maxHealthPoints;
    [SerializeField] protected float currentHealthPoints;
    [SerializeField] protected float speed;
    [SerializeField] protected float linearDamping;
    [SerializeField] protected DirectionHandler directionHandler;

    [Header("Entity components")]
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidbody;
    protected Animator animator;
    protected CapsuleCollider2D collider;
    protected Material material;


    public void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<CapsuleCollider2D>();
        material = spriteRenderer.material;
        currentHealthPoints = maxHealthPoints;
        if (rigidbody != null)
        {
            rigidbody.linearDamping = linearDamping;
        } 
    }

}

public class DirectionHandler
{
    private List<Vector3> Directions = new List<Vector3> { Vector3.up, Vector3.left, Vector3.down, Vector3.right };
    [SerializeField] private Vector3 currentDirection = Vector3.down;
}
