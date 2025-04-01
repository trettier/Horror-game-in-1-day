using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Environment : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite defaultSprite;
    private Rigidbody2D rb;
    private float time;

    [SerializeField] private Sprite outlinedSprite; 
    [SerializeField] private bool IsCollect = false;
    [SerializeField] private AudioSource deny;
    private bool isTakingDamage = false;  // ������� ����, ����� �������������� ���������
    private float flashTime = 0.25f;
    private Material material;
    private bool playerIsNear = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
        material = spriteRenderer.material;
    }

    void Update()
    {

        if (playerIsNear &&  Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (IsCollect) 
            {
                SceneManager.LoadScene("Final");
            }
            else
            {
                deny.Play();
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerIsNear = true;
            spriteRenderer.sprite = outlinedSprite;

        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerIsNear = false;
            spriteRenderer.sprite = defaultSprite;

        }
    }

    public void FixPortal()
    {
        IsCollect = true;
    }

}

