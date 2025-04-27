using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IAnimatorController
{
    void UpdateAnimation(Vector2 direction, bool _isMoving = true);

    void Initialize(Animator animator, SpriteRenderer spriteRenderer, Material material);

    void SpeedUp(float speedUp);

    void SlowDown();

    void Flash();

    bool Param();
}
