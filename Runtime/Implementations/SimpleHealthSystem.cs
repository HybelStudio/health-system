using UnityEngine;
using System;

namespace Hybel.HealthSystem
{
    public class SimpleHealthSystem : ISimpleHealthSystem
    {
        public event SimpleHealthChange HealthReduced;
        public event SimpleHealthChange HealthReplenished;
        public event Action<float> HealthChanged;
        public event Action HealthReachedZero;

        private float _currentHealth;
        private readonly float _maxHealth;

        public SimpleHealthSystem(float startingHealth) : this(startingHealth, startingHealth) { }
        public SimpleHealthSystem(float startinHealth, float maxHealth)
        {
            _maxHealth = Mathf.Max(maxHealth, 0f);
            _currentHealth = Mathf.Clamp(startinHealth, 0f, _maxHealth);
        }

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;

        public float ReduceHealth(float amountToReduceBy)
        {
            amountToReduceBy = Mathf.Max(amountToReduceBy, 0f);
            float maxAmountToReduce = Mathf.Min(_currentHealth, amountToReduceBy);
            _currentHealth -= maxAmountToReduce;

            HealthReduced?.Invoke(maxAmountToReduce);
            HealthChanged?.Invoke(_currentHealth);

            if (_currentHealth <= 0f && maxAmountToReduce > 0f)
                HealthReachedZero?.Invoke();

            return maxAmountToReduce;
        }

        public float ReplenishHealth(float amountToReplenish)
        {
            amountToReplenish = Mathf.Max(amountToReplenish, 0f);
            float maxAmountToReplenish = Mathf.Min(_maxHealth - _currentHealth, amountToReplenish);

            if (maxAmountToReplenish <= 0f)
                return 0f;

            _currentHealth += maxAmountToReplenish;
            HealthReplenished?.Invoke(maxAmountToReplenish);
            HealthChanged?.Invoke(_currentHealth);

            return maxAmountToReplenish;
        }
    }
}
