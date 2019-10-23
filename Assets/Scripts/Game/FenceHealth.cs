using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FenceHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    private Slider _healthSlider;

    public UnityAction OnFenceDestroyed;
    public UnityAction OnFenceDamaged;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    private void OnEnable()
    {
        RestoreHealth();
    }

    public void SetHealthSlider(Slider healthSlider)
    {
        _healthSlider = healthSlider;

        UpdateHealthBar();
    }

    public void DealDamage(float damage)
    {
        if (!IsFenceDamaged())
        {
            OnFenceDamaged?.Invoke();
        }

        _currentHealth -= damage;
        

        if (_currentHealth <= 0f)
        {
            OnFenceDestroyed?.Invoke();
        }

        UpdateHealthBar();
    }

    public bool IsFenceDamaged()
    {
        return Math.Abs(_currentHealth - _maxHealth) > 0;
    }

    public void RestoreHealth()
    {
        _currentHealth = _maxHealth;
    }

    public float GetCurrentHealthPoints()
    {
        return _currentHealth;
    }

    public void UpdateHealthBar()
    {      
        float healthPercentage = _currentHealth / _maxHealth;
        _healthSlider.value = healthPercentage;
    }
}
