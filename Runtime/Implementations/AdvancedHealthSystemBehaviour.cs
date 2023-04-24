using System;
using UnityEngine;
using UnityEngine.Events;
using Hybel.HealthSystem;

public class AdvancedHealthSystemBehaviour : HealthSystemBehaviour, IAdvancedHealthSystem
{
    public event HealthChange HealthReduced;
    public event HealthChange HealthReplenished;
    public event Action<float> HealthChanged;
    public event Action HealthReachedZero;

    [SerializeField] private int startingHealth;

    public UnityEvent<IHealthDifference> OnHealthReduced;
    public UnityEvent<IHealthDifference> OnHealthReplenished;
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent OnHealthReachedZero;

    public IHealthPool HealthPool => healthSystem.HealthPool;

    private new AdvancedHealthSystem healthSystem => base.healthSystem as AdvancedHealthSystem;

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

    private void OnGUI()
    {
        if (GUILayout.Button("-15"))
        {
            ReduceHealth(15);
        }
            
        if (GUILayout.Button("+15"))
        {
            ReplenishHealth(15);
        }
    }

    public IHealthDifference ReduceHealthAdvanced(float amountToReduceBy) => healthSystem.ReduceHealthAdvanced(amountToReduceBy);
    public IHealthDifference ReduceHealthAdvanced(float amountToReduceBy, IHealthType healthType) => healthSystem.ReduceHealthAdvanced(amountToReduceBy, healthType);
    public IHealthDifference ReduceHealthAdvanced(IHealth healthToReduceBy) => healthSystem.ReduceHealthAdvanced(healthToReduceBy);
    public IHealthDifference ReduceHealthAdvanced(IHealthPool healthPoolToReduceBy) => healthSystem.ReduceHealthAdvanced(healthPoolToReduceBy);

    public IHealthDifference ReplenishHealthAdvanced(float amountToReplenish) => healthSystem.ReplenishHealthAdvanced(amountToReplenish);
    public IHealthDifference ReplenishHealthAdvanced(float amountToReduceBy, IHealthType healthType) => healthSystem.ReplenishHealthAdvanced(amountToReduceBy, healthType);
    public IHealthDifference ReplenishHealthAdvanced(IHealth healthToReplenish) => healthSystem.ReplenishHealthAdvanced(healthToReplenish);
    public IHealthDifference ReplenishHealthAdvanced(IHealthPool healthPoolToReduceBy) => healthSystem.ReplenishHealthAdvanced(healthPoolToReduceBy);

    protected override IHealthSystem GetHealthSystem() => new AdvancedHealthSystem(new Health(startingHealth));

    private void InvokeOnHealthReduced(IHealthDifference healthDifference)
    {
        HealthReduced?.Invoke(healthDifference);
        OnHealthReduced?.Invoke(healthDifference);
    }

    private void InvokeOnHealthReplenished(IHealthDifference healthDifference)
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
