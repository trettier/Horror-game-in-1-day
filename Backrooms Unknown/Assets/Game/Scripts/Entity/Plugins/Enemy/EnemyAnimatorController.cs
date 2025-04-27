using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAnimatorController : MonoBehaviour, IAnimatorController
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _direction;
    private Material _material;
    private bool _isMoving = false;
    [SerializeField] private float _flashTime = 0.25f;
    public void Initialize(Animator animator, SpriteRenderer spriteRenderer, Material material)
    {
        _animator = animator;
        _spriteRenderer = spriteRenderer;
        _material = material;
    }

    public void UpdateAnimation(Vector2 direction, bool isMoving)
    {
        _isMoving = isMoving;
        _direction = direction;
        _animator.SetBool("Move", _isMoving);

        if (_direction.x > 0)
        {
            _spriteRenderer.flipX = false;
            _isMoving = true;
        }
        else if (_direction.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }

    public void SpeedUp(float speedUp)
    {
        _animator.speed = speedUp;
    }

    public void SlowDown()
    {
        _animator.speed = 1f;
    }

    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    public bool Param()
    {
        return true;
    }

    public IEnumerator FlashCoroutine()
    {

        float currentFlash = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlash = Mathf.Lerp(1f, 0f, elapsedTime / _flashTime);
            _material.SetFloat("Flash", currentFlash);
            yield return null;
        }
    }
}