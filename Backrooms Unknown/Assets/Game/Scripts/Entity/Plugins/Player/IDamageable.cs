using UnityEngine;
using System;

public interface IDamageable
{
    event Action Death;

    event Action TakeDamage;

    void Initialize(float maxHealthPoints);

    void ReceiveDamage(float damage);
}
