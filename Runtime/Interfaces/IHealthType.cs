using System;

namespace Hybel.HealthSystem
{
    public interface IHealthType : IEquatable<IHealthType>
    {
        public int Order { get; }

        /// <summary>
        /// Implement this if you wish to do logic on every frame.
        /// <para>This is not guaranteed to run when using a custom <see cref="IHealthSystem>"/> implementation.</para>
        /// </summary>
        /// <param name="currentAmount">The current amount of health on this frame.</param>
        /// <param name="maxAmount">The max amount of health this health type can have.</param>
        /// <returns>The new amount of health to be assign.</returns>
        public float OnUpdate(float currentAmount, float maxAmount);

        /// <summary>
        /// Implement this if you wish to alter how this health type handles reducing health.
        /// </summary>
        /// <param name="originalAmountToReduceBy">The original amount to reduce by before any modifications.</param>
        /// <returns>The new amount to reduce by.</returns>
        public float OnModifyReduceHealth(float originalAmountToReduceBy);

        /// <summary>
        /// Implement this if you wish to alter how this health type handles replenishing health.
        /// </summary>
        /// <param name="originalAmountToReplenish">The original amount to replenish by before any modifications.</param>
        /// <returns>The new amount to replenish by.</returns>
        public float OnModifyReplensihHealth(float originalAmountToReplenish);
    }
}
