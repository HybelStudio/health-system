using System;
using UnityEngine;
using UnityEngine.Events;
using Hybel.HealthSystem;

public class SimpleHealthSystemBehaviour : HealthSystemBehaviour, ISimpleHealthSystem
{
    public event SimpleHealthChange HealthReduced;
    public event SimpleHealthChange HealthReplenished;
    public event Action<float> HealthChanged;
    public event Action HealthReachedZero;

    public UnityEvent<float> OnHealthReduced;
    public UnityEvent<float> OnHealthReplenished;
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent OnHealthReachedZero;

    [SerializeField] private float startingHealth;

    private new SimpleHealthSystem healthSystem => base.healthSystem as SimpleHealthSystem;

    protected sealed override void Awake()
    {
        base.Awake();

        healthSystem.HealthReduced += InvokeOnHealthReduced;
        healthSystem.HealthReplenished += InvokeOnHealthReplenished;
        healthSystem.HealthChanged += InvokeOnHealthChanged;
        healthSystem.HealthReachedZero += InvokeOnHealthReachedZero;
    }

    private void OnDestroy()
    {
        healthSystem.HealthReduced -= InvokeOnHealthReduced;
        healthSystem.HealthReplenished -= InvokeOnHealthReplenished;
        healthSystem.HealthChanged -= InvokeOnHealthChanged;
        healthSystem.HealthReachedZero -= InvokeOnHealthReachedZero;
    }

    protected sealed override IHealthSystem GetHealthSystem() => new SimpleHealthSystem(startingHealth);

    private void InvokeOnHealthReduced(float healthDifference)
    {
        HealthReduced?.Invoke(healthDifference);
        OnHealthReduced?.Invoke(healthDifference);
    }

    private void InvokeOnHealthReplenished(float healthDifference)
    {
        HealthReplenished?.Invoke(healthDifference);
        OnHealthReplenished?.Invoke(healthDifference);
    }

    private void InvokeOnHealthChanged(float newCurrentHealth)
    {
        HealthChanged?.Invoke(newCurrentHealth);
        OnHealthChanged?.Invoke(newCurrentHealth);
    }

    private void InvokeOnHealthReachedZero()
    {
        HealthReachedZero?.Invoke();
        OnHealthReachedZero?.Invoke();
    }
}
