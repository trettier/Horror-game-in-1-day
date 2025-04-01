using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private GameObject _player;
    private Vector2 _movement;
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private int _speed;
    [SerializeField]
    private int increaseParam;

    [SerializeField] private List<Sprite> frames;
    private int frame = 0;
    private float lastFrameTime = 0;
    [SerializeField] private AudioSource takeDamageSound;
    [SerializeField] private AudioSource footstepSound;
    [SerializeField] private AudioSource scream;
    [SerializeField] private List<Image> heartIndicators;
    [SerializeField] private int HP = 3;
    [SerializeField] private GameObject DeathScreen;
    private int MaxHP;

    public int InPool = 0;
    [SerializeField]
    private GameObject particleSystem;

    public delegate void DeathEventHandler();
    public event DeathEventHandler OnDeathEvent;

    public delegate void CollectEventHandler();
    public event CollectEventHandler OnCollectEvent;

    private float flashTime = 0.25f;
    private Material material;
    private Animator animator;
    private bool _isMoving = false;

    public void __init__(GameObject PlayerObject)
    {
        _player = PlayerObject;
        _rb = PlayerObject.GetComponent<Rigidbody2D>();
        spriteRenderer = PlayerObject.GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        animator = PlayerObject.GetComponent<Animator>();
        MaxHP = heartIndicators.Count;
        //InterfaceUpdate();
    }


    void FixedUpdate()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        _movement = _movement.normalized;

        // Обработка поворота спрайта
        if (_movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (_movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        // Проверка, движется ли игрок
        bool wasMoving = _isMoving;
        _isMoving = (_movement.x != 0 || _movement.y != 0);

        // Анимация движения
        animator.SetBool("Move", _isMoving);

        // Управление звуком шагов
        if (_isMoving)
        {
            if (!wasMoving) // Если только начали движение
            {
                footstepSound.Play(); // Запускаем звук
            }
        }
        else if (wasMoving) // Если только остановились
        {
            footstepSound.Stop(); // Останавливаем звук
        }

    _rb.AddForce(_movement * _speed);

    }

    private void TakeDamage()
    {
        StartCoroutine(Flash());
        if (HP > 1)
        {
            HP--;
        }
        else
        {
            Death();
        }
        //InterfaceUpdate();
        //takeDamageSound.Play();
        StartCoroutine(Camera.main.GetComponent<ScreenShake>().Shake(0.3f, 0.05f));
    }

    public void IncreaseSpeed()
    {
        if (InPool == 0)
        {
            _rb.AddForce(_movement * _speed * increaseParam);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //if (collider.CompareTag("Collectable"))
        //{
        //    Collect(collider.gameObject);
        //}
        //if (collider.CompareTag("Floor"))
        //{
        //    _speed *= 2;
        //    InPool = 0;
        //    frame = (frame) % 2 + InPool;
        //    spriteRenderer.sprite = frames[frame];
        //}
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        //if (collider.CompareTag("Floor"))
        //{
        //    _speed /= 2;
        //    InPool = 2;
        //    frame = (frame) % 2 + InPool;
        //    spriteRenderer.sprite = frames[frame];
        //}
    }

    private void InterfaceUpdate()
    {
        for (int i = 0; i < HP; i++)
        {
            heartIndicators[i].gameObject.SetActive(true);
        }
        for (int i = HP; i < MaxHP; i++)
        {
            heartIndicators[i].gameObject.SetActive(false);
        }
    }

    public void Death()
    {
        StartCoroutine(DeathWithDelay());
    }

    private IEnumerator DeathWithDelay()
    {
        scream.Play();

        yield return new WaitForSeconds(scream.clip.length / 2);

        OnDeathEvent?.Invoke();
    }

    public void Collect(GameObject collectable)
    {
        Destroy(collectable);
        OnCollectEvent?.Invoke();
    }

    private IEnumerator Flash()
    {

        float currentFlash = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlash = Mathf.Lerp(1f, 0f, elapsedTime / flashTime);
            material.SetFloat("Flash", currentFlash);
            yield return null;
        }
    }
}
