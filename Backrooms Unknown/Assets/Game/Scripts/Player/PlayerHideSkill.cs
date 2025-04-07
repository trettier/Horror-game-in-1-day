using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHideSkill : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer shadowSpriteRenderer;
    [SerializeField] private GameObject lighter;
    private bool nearBox = false;
    private PlayerController playerController;
    private CapsuleCollider2D playerCollider2D;
    private Vector3 playerPositionBeforeHide;
    private Vector3 objectToHidePosition;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        playerCollider2D = GetComponent<CapsuleCollider2D>();
    }

    public bool TryToHide(bool inbox)
    {
        if (!inbox)
        {
            EnterTheBox();
        }
        else
        {
            ExitTheBox();
        }
        return !inbox;
    }

    private void EnterTheBox()
    {
        spriteRenderer.enabled = false;
        shadowSpriteRenderer.enabled = false; 
        playerCollider2D.enabled = false;
        lighter.SetActive(false);

        gameObject.tag = "PlayerHidden";

        playerPositionBeforeHide = transform.position;
        transform.position = objectToHidePosition;
    }

    private void ExitTheBox()
    {
        spriteRenderer.enabled = true;
        shadowSpriteRenderer.enabled = true;
        playerCollider2D.enabled = true;
        lighter.SetActive(true);

        gameObject.tag = "Player";
        transform.position = playerPositionBeforeHide;
    }

    public void SetObjectToHidePosition(Vector3 pos)
    {
        objectToHidePosition = pos;
    }

}

