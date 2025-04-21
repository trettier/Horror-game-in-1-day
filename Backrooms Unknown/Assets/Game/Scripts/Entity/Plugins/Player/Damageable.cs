using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Damageable : MonoBehaviour, IDamageable
{
    private float _maxHealthPoints;
    private float _currentHealthPoints;

    public event Action Death;
    public event Action TakeDamage;

    public void Initialize(float maxHealthPoints)
    {
        _maxHealthPoints = maxHealthPoints;
        _currentHealthPoints = maxHealthPoints;
    }

    public void ReceiveDamage(float damage)
    {
        _currentHealthPoints -= damage;
        TakeDamage?.Invoke();

        if (_currentHealthPoints < 0)
        {
            Death?.Invoke();
        }
    }
}
