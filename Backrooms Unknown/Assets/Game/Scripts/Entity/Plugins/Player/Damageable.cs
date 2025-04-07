using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Damageable : MonoBehaviour, IDamageable
{
    IAnimatorController _animatorController;
    ISoundController _soundController;
    private float _maxHealthPoints;
    private float _currentHealthPoints;

    public event Action OnDeath;

    public void Initialize(IAnimatorController animatorController, ISoundController soundController, float maxHealthPoints)
    {
        _animatorController = animatorController;
        _maxHealthPoints = maxHealthPoints;
        _currentHealthPoints = maxHealthPoints;
        _soundController = soundController;
    }

    public void RecieveDamage(float damage)
    {
        _currentHealthPoints -= damage;
        _animatorController.Flash();
        _soundController.MakeHitSound();

        if (_currentHealthPoints < 0)
        {
            OnDeath?.Invoke();
        }
    }
}
