using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Hybel.HealthSystem
{
    public class AdvancedHealthSystem : IAdvancedHealthSystem
    {
        public event HealthChange HealthReduced;
        public event HealthChange HealthReplenished;
        public event Action<float> HealthChanged;
        public event Action HealthReachedZero;

        private readonly Option<IHealthPool> _healthPool;
        private readonly Option<IReadonlyHealthPool> _maxHealthPool;

        public AdvancedHealthSystem(IHealth health) : this(new HealthPool(health), new HealthPool(health)) { }
        public AdvancedHealthSystem(IHealthPool healthPool) : this(healthPool, healthPool) { }
        public AdvancedHealthSystem(IHealth startingHealth, IHealth maxHealth) : this(new HealthPool(startingHealth), new HealthPool(maxHealth)) { }
        public AdvancedHealthSystem(IHealthPool startingHealthPool, IHealthPool maxHealthPool)
        {
            maxHealthPool = SimplifyHealthPool(maxHealthPool);

            if (startingHealthPool != null)
            {
                IEnumerable<IGrouping<IHealthType, IHealth>> groups = startingHealthPool.GroupBy(health => health.HealthType);

                List<IHealth> healths = new();
                foreach (IGrouping<IHealthType, IHealth> group in groups)
                {
                    IHealthType healthType = group.Key;

                    float currentAmount = 0f;

                    foreach (IHealth element in group)
                        currentAmount += element.Amount;

                    IHealth matchingMaxHealth = maxHealthPool.FirstOrDefault(health => health.HealthType == healthType);

                    if (matchingMaxHealth == null)
                    {
                        matchingMaxHealth = new Health(currentAmount, healthType);
                        maxHealthPool.Healths.Add(matchingMaxHealth);
                    }

                    currentAmount = Mathf.Min(currentAmount, matchingMaxHealth.Amount);
                    healths.Add(new Health(currentAmount, healthType));
                }

                _healthPool = new HealthPool(healths);
                _maxHealthPool = new Option<IReadonlyHealthPool>(maxHealthPool.ToReadOnly());
                return;
            }

            _healthPool = Option<IHealthPool>.None;
        }

        public IHealthPool HealthPool => _healthPool.DangerousValue;
        public IReadonlyHealthPool MaxHealthPool => _maxHealthPool.DangerousValue;

        public float CurrentHealth
        {
            get
            {
                if (_healthPool.TryUnwrap(out IHealthPool healthPool))
                    if (healthPool.Healths != null)
                        return Sum(healthPool.Healths.Select(health => health.Amount));

                return 0f;
            }
        }

        public float MaxHealth
        {
            get
            {
                if (_maxHealthPool.TryUnwrap(out IReadonlyHealthPool healthPool))
                    if (healthPool.Healths != null)
                        return Sum(healthPool.Healths.Select(health => health.Amount));

                return 0f;
            }
        }

        public void OnUpdate()
        {
            if (!_healthPool.TryUnwrap(out IHealthPool healthPool))
                return;

            foreach (var health in healthPool)
            {
                var maxHealthOption = FindMaxHealth(health);

                if (!maxHealthOption.TryUnwrap(out IReadonlyHealth maxHealth))
                    continue;

                float newHealth = health.HealthType.OnUpdate(health.Amount, maxHealth.Amount);
                health.Amount = newHealth;
            }
        }

        public float ReduceHealth(float amountToReduceBy) => ReduceHealthAdvanced(amountToReduceBy).TotalDifference;

        public IHealthDifference ReduceHealthAdvanced(float amountToReduceBy)
        {
            if (!_healthPool.TryUnwrap(out IHealthPool healthPool))
                return new HealthDifference();

            float remainingAmountToReduceBy = Mathf.Max(amountToReduceBy, 0f);

            if (remainingAmountToReduceBy == 0f)
                return new HealthDifference();

            float totalBeforeReduction = CurrentHealth;

            List<IHealth> reducedHealths = new();
            foreach (IHealth health in healthPool)
            {
                if (health.HealthType != null)
                    remainingAmountToReduceBy = health.HealthType.OnModifyReduceHealth(remainingAmountToReduceBy);

                float maxPossibleAmountToReduce = Mathf.Min(remainingAmountToReduceBy, health.Amount);

                if (maxPossibleAmountToReduce == 0f)
                    continue;

                health.Amount -= maxPossibleAmountToReduce;

                IHealth reducedHealth = new Health(maxPossibleAmountToReduce, health.HealthType);
                reducedHealths.Add(reducedHealth);

                if (maxPossibleAmountToReduce <= remainingAmountToReduceBy)
                {
                    var healthDifference = new HealthDifference(reducedHealths);
                    HealthReduced?.Invoke(healthDifference);
                    HealthChanged?.Invoke(CurrentHealth);

                    if (totalBeforeReduction > 0f && totalBeforeReduction - healthDifference.TotalDifference <= 0f)
                        HealthReachedZero?.Invoke();

                    return healthDifference;
                }

                remainingAmountToReduceBy -= maxPossibleAmountToReduce;
            }

            return new HealthDifference();
        }

        public IHealthDifference ReduceHealthAdvanced(float amountToReduceBy, IHealthType healthType) =>
            ReduceHealthAdvanced(new Health(amountToReduceBy, healthType));

        public IHealthDifference ReduceHealthAdvanced(IHealth healthToReduceBy) => ReduceHealthAdvanced(healthToReduceBy, true);

        public IHealthDifference ReduceHealthAdvanced(IHealthPool healthPoolToReduceBy)
        {
            List<IHealth> healthsReduced = new();

            foreach (IHealth healthToReduceBy in healthPoolToReduceBy)
                healthsReduced.AddRange(ReduceHealthAdvanced(healthToReduceBy, false));

            HealthDifference healthDifference;
            if (healthsReduced.Count > 0)
            {
                healthDifference = new HealthDifference(healthsReduced);
                HealthReduced?.Invoke(healthDifference);
                HealthChanged?.Invoke(CurrentHealth);
            }
            else
            {
                healthDifference = new HealthDifference();
            }

            return healthDifference;
        }

        private IHealthDifference ReduceHealthAdvanced(IHealth healthToReduceBy, bool invokeEvents)
        {
            if (healthToReduceBy != null && _healthPool.TryUnwrap(out IHealthPool healthPool))
            {
                List<IHealth> reducedHealths = new();
                foreach (IHealth health in healthPool)
                {
                    if (!HealthTypesAreEqual(health, healthToReduceBy))
                        continue;

                    float maxPossibleAmountToReduce = Mathf.Min(healthToReduceBy.Amount, health.Amount);

                    if (maxPossibleAmountToReduce <= 0f)
                        continue;

                    health.Amount -= maxPossibleAmountToReduce;

                    IHealth reducedHealth = new Health(maxPossibleAmountToReduce, health.HealthType);
                    reducedHealths.Add(reducedHealth);

                    healthToReduceBy.Amount -= maxPossibleAmountToReduce;
                }

                if (reducedHealths.Count > 0)
                {
                    var healthDifference = new HealthDifference(reducedHealths);

                    if (invokeEvents)
                    {
                        HealthReduced?.Invoke(healthDifference);
                        HealthChanged?.Invoke(CurrentHealth);
                    }

                    return healthDifference;
                }
            }

            return new HealthDifference();
        }

        public float ReplenishHealth(float amountToReplenish) => ReplenishHealthAdvanced(amountToReplenish).TotalDifference;

        public IHealthDifference ReplenishHealthAdvanced(float amountToReplenish) =>
            ReplenishHealthAdvanced(new Health(amountToReplenish), false);

        public IHealthDifference ReplenishHealthAdvanced(float amountToReplenish, IHealthType healthType) =>
            ReplenishHealthAdvanced(new Health(amountToReplenish, healthType), false);

        public IHealthDifference ReplenishHealthAdvanced(IHealth healthToReplenish) => ReplenishHealthAdvanced(healthToReplenish, false);

        public IHealthDifference ReplenishHealthAdvanced(IHealthPool healthPoolToReplenish)
        {
            List<IHealth> healthsReplenished = new();

            foreach (IHealth healthToReplenish in healthPoolToReplenish)
                healthsReplenished.AddRange(ReplenishHealthAdvanced(healthToReplenish, false));

            return healthsReplenished.Count > 0 ? new HealthDifference(healthsReplenished) : new HealthDifference();
        }

        private IHealthDifference ReplenishHealthAdvanced(IHealth healthToReplenish, bool allowOverflow)
        {
            if (healthToReplenish != null && _healthPool.TryUnwrap(out IHealthPool healthPool))
            {
                float amountReplenished = 0f;
                if (healthPool.Healths != null)
                {
                    List<IHealth> healths = healthPool.Healths;
                    if (healths.Count > 0 && healths.Any(h => h.HealthType == healthToReplenish.HealthType))
                    {
                        foreach (IHealth health in healths)
                            if (health.HealthType == healthToReplenish.HealthType)
                                amountReplenished += MergeHealth(healthToReplenish, health, new Option<IHealthType>(health.HealthType), allowOverflow);
                    }
                    else
                    {
                        if (allowOverflow)
                        {
                            healthPool.AddHealth(healthToReplenish);
                            amountReplenished += healthToReplenish.Amount;
                        }
                    }
                }

                var healthDifference = new HealthDifference(new List<IHealth> { new Health(amountReplenished, healthToReplenish.HealthType, healthToReplenish.Order) });

                if (healthDifference.TotalDifference > 0f)
                {
                    HealthReplenished?.Invoke(healthDifference);
                    HealthChanged?.Invoke(CurrentHealth);
                }

                return healthDifference;
            }

            return new HealthDifference();
        }

        private float MergeHealth(IHealth source, IHealth destination, Option<IHealthType> healthTypeOption, bool allowOverflow)
        {
            if (_healthPool.TryUnwrap(out IHealthPool actualHealthPool) && _maxHealthPool.TryUnwrap(out IReadonlyHealthPool actualMaxHealthPool))
            {
                if (actualHealthPool.Healths != null && actualMaxHealthPool.Healths != null)
                {
                    int index = actualHealthPool.Healths.IndexOf(destination);
                    float targetAmount = source.Amount;

                    if (healthTypeOption.TryUnwrap(out IHealthType healthType))
                        targetAmount = healthType.OnModifyReplensihHealth(source.Amount);

                    float maxAmountOfHealthThatCanBeAdded = Mathf.Min(actualMaxHealthPool.Healths[index].Amount - destination.Amount, targetAmount);
                    destination.Amount += maxAmountOfHealthThatCanBeAdded;
                    return maxAmountOfHealthThatCanBeAdded;
                }

                return 0f;
            }

            destination.Amount += source.Amount;
            return source.Amount;
        }

        private static IHealthPool SimplifyHealthPool(IHealthPool healthPool)
        {
            List<IHealth> healths = new();

            IEnumerable<IGrouping<IHealthType, IHealth>> groups = healthPool.GroupBy(h => h.HealthType);

            foreach (IGrouping<IHealthType, IHealth> group in groups)
            {
                IHealthType healthType = group.Key;

                float currentAmount = 0f;

                foreach (IHealth element in group)
                    currentAmount += element.Amount;

                healths.Add(new Health(currentAmount, healthType));
            }

            return new HealthPool(healths);
        }

        private bool HealthTypesAreEqual(IHealth left, IHealth right) =>
            (left.HealthType is null && right.HealthType is null) || left.HealthType.Equals(right.HealthType);

        private static float Sum(IEnumerable<float> values)
        {
            float sum = 0f;

            foreach (float value in values)
                sum += value;

            return sum;
        }

        private Option<IReadonlyHealth> FindMaxHealth(IHealth health)
        {
            if (health is null)
                return Option<IReadonlyHealth>.None;

            if (!_maxHealthPool.TryUnwrap(out IReadonlyHealthPool maxHealthPool))
                return Option<IReadonlyHealth>.None;

            foreach (IReadonlyHealth maxHealth in maxHealthPool)
                if (maxHealth.HealthType.Equals(health.HealthType))
                    return maxHealth.Some();

            return Option<IReadonlyHealth>.None;
        }
    }
}
