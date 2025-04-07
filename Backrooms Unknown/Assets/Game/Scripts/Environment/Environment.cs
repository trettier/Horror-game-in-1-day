using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite defaultSprite;
    private Rigidbody2D rb;
    private float time;

    [SerializeField] private Sprite outlinedSprite; 
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

    public void SetOutlinedSprite()
    {
        spriteRenderer.sprite = outlinedSprite;
    }

    public void SetDefaultSprite()
    {
        spriteRenderer.sprite = defaultSprite;
    }


}

