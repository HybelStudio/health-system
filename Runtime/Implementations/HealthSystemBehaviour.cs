using UnityEngine;

namespace Hybel.HealthSystem
{
    public abstract class HealthSystemBehaviour : MonoBehaviour, IHealthSystem
    {
        private IHealthSystem _healthSystem;

        public float CurrentHealth => _healthSystem.CurrentHealth;
        public float MaxHealth => _healthSystem.MaxHealth;

        protected IHealthSystem healthSystem => _healthSystem;

        protected virtual void Awake() => _healthSystem = GetHealthSystem();

        protected abstract IHealthSystem GetHealthSystem();

        public float ReduceHealth(float amountToReduceBy) => _healthSystem.ReduceHealth(amountToReduceBy);
        public float ReplenishHealth(float amountToReplenish) => _healthSystem.ReplenishHealth(amountToReplenish);
    }
}
