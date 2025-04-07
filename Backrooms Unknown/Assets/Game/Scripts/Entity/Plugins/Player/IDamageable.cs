using UnityEngine;
using System;

public interface IDamageable
{
    event Action OnDeath;

    void Initialize(IAnimatorController animatorController, ISoundController soundController, float maxHealthPoints);

    void RecieveDamage(float damage);
}
