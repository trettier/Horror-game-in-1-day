using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int increaseParam;
    [SerializeField]
    protected Vector3 direction;
    [SerializeField]
    protected float hp;
    [SerializeField]
    protected bool patrolObject = false;
    [SerializeField]
    protected List<Vector3> patrolPoints;
    [SerializeField]
    protected List<PatrolPoint> patrolObjects;
    protected int currentPoint = 1;
    protected PatrolPoint currentObject;
    protected float lastFrameTime;
    [SerializeField] protected float rotationTime = 0.3f;
    protected int flagDirection;

    [SerializeField]
    public EnemyState currentState = EnemyState.Spawning;
    protected List<Vector3> directions = new List<Vector3> { Vector3.up, Vector3.left, Vector3.down, Vector3.right };
    protected Transform playerPosition;
    protected Transform position;
    protected Transform player;
    protected SpriteRenderer spriteRenderer;

    [SerializeField]
    protected float viewAngle = 120f; // Угол обзора
    [SerializeField]
    protected float viewDistance = 10f; // Дистанция обзора
    protected bool playerInVision = false;
    protected Vector3 chasePoint;
    protected Rigidbody2D rb;
    protected bool takeDamage = false;
    protected bool InPool = false;
    protected float flashTime = 0.25f;
    protected Material material;


    protected Animator animator;
    [SerializeField]
    protected AudioSource takeDamageSound;
    [SerializeField]
    protected GameObject particleSystem;
    [SerializeField]
    protected Lighter lighter;
    protected bool _isMoving = false;
    protected bool _isFootstepSoundPlaying = false;
    protected bool playerIsNear = false;
    [SerializeField] protected AudioSource footstepSound;
    [SerializeField] protected AudioSource Laughter;
    [SerializeField] protected float maxDistance = 10;

    void Start()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentState = EnemyState.Spawning;
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = directions[Random.Range(0, 4)];
        material = spriteRenderer.material;
        if (SceneManager.GetActiveScene().name == "Level 2_1")
        {
            rotationTime = 1;
        }
    }

}

public enum EnemyState { Idle, Patrolling, Investigating, Chasing, Spawning, Dying }