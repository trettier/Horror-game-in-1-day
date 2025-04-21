using UnityEngine;
using System;

public interface IBehavior
{
    void Initialize(IMovementController movementController, IAnimatorController animatorController, IDamageable damageable, ISoundController soundController);

    void UpdateAction();
}