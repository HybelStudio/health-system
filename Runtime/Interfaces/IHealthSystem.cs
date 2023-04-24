using System;

namespace Hybel.HealthSystem
{
    public interface IHealthSystem
    {
        public float CurrentHealth { get; }
        public float MaxHealth { get; }

        public float ReduceHealth(float amountToReduceBy);
        public float ReplenishHealth(float amountToReplenish);
    }

    public delegate void HealthChange(IHealthDifference healthDifference);

    public interface IAdvancedHealthSystem : IHealthSystem
    {
        /// <summary>
        /// Pass a positive number that represents the amount of health reduced.
        /// </summary>
        public event HealthChange HealthReduced;

        /// <summary>
        /// Pass a positive number that represents the amount of health replenished.
        /// </summary>
        public event HealthChange HealthReplenished;

        /// <summary>
        /// Pass a positive number that represents the amount of health replenished.
        /// </summary>
        public event Action<float> HealthChanged;

        /// <summary>
        /// Invoked when the <see cref="IHealthSystem.CurrentHealth"/> value has become zero or less.
        /// </summary>
        public event Action HealthReachedZero;

        public IHealthPool HealthPool { get; }

        public IHealthDifference ReduceHealthAdvanced(float amountToReduceBy);
        public IHealthDifference ReduceHealthAdvanced(float amountToReduceBy, IHealthType healthType);
        public IHealthDifference ReduceHealthAdvanced(IHealth healthToReduceBy);
        public IHealthDifference ReduceHealthAdvanced(IHealthPool healthPoolToReduceBy);

        public IHealthDifference ReplenishHealthAdvanced(float amountToReplenish);
        public IHealthDifference ReplenishHealthAdvanced(float amountToReduceBy, IHealthType healthType);
        public IHealthDifference ReplenishHealthAdvanced(IHealth healthToReplenish);
        public IHealthDifference ReplenishHealthAdvanced(IHealthPool healthPoolToReduceBy);
    }

    public delegate void SimpleHealthChange(float healthDifference);

    public interface ISimpleHealthSystem : IHealthSystem
    {
        /// <summary>
        /// Pass a positive number that represents the amount of health reduced.
        /// </summary>
        public event SimpleHealthChange HealthReduced;

        /// <summary>
        /// Pass a positive number that represents the amount of health replenished.
        /// </summary>
        public event SimpleHealthChange HealthReplenished;

        /// <summary>
        /// Pass a signed number that represents the difference between the health before and after changing.
        /// </summary>
        public event Action<float> HealthChanged;

        public event Action HealthReachedZero;
    }
}

//using System;

//namespace Hybel.Health
//{
//    public delegate void HealthChange(IHealthDifference healthDifference);

//    public interface IHealthSystem
//    {
//        public event HealthChange HealthReduced;
//        public event HealthChange HealthReplenished;
//        public event Action<float> HealthChanged;
//        public event Action HealthReachedZero;

//        public IHealthPool HealthPool { get; }
//        public float CurrentHealth { get; }
//        public float MaxHealth { get; }

//        public IHealthDifference ReduceHealth(float amountToReduceBy);
//        public IHealthDifference ReduceHealth(float amountToReduceBy, IHealthType healthType);
//        public IHealthDifference ReduceHealth(IHealth healthToReduceBy);
//        public IHealthDifference ReduceHealth(IHealthPool healthPoolToReduceBy);

//        public IHealthDifference ReplenishHealth(float amountToReplenish);
//        public IHealthDifference ReplenishHealth(float amountToReduceBy, IHealthType healthType);
//        public IHealthDifference ReplenishHealth(IHealth healthToReplenish);
//        public IHealthDifference ReplenishHealth(IHealthPool healthPoolToReduceBy);
//    }

//    public delegate void SimpleHealthChange(float healthDifference);

//    public interface ISimpleHealthSystem
//    {
//        public event SimpleHealthChange HealthReduced;
//        public event SimpleHealthChange HealthReplenished;
//        public event Action<float> HealthChanged;
//        public event Action HealthReachedZero;

//        public float CurrentHealth { get; }
//        public float MaxAmount { get; }

//        public float ReduceHealth(float amountToReduceBy);
//        public float ReplenishHealth(float amountToReplenish);
//    }
//}

