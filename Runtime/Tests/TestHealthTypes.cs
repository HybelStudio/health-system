using UnityEngine;

namespace Hybel.HealthSystem.Tests
{
    public struct Armor : IHealthType
    {
        public int Order => 1;

        public bool Equals(IHealthType other) => other is Armor armor && Order == armor.Order;

        public float OnModifyReduceHealth(float originalAmountToReduceBy) => originalAmountToReduceBy - 2f;
        public float OnModifyReplensihHealth(float originalAmountToReplenish) => originalAmountToReplenish;
        public float OnUpdate(float currentAmount, float maxAmount) => currentAmount;
    }

    public class SuperArmor : IHealthType
    {
        public int Order => 2;

        public bool Equals(IHealthType other) => other is SuperArmor armor && Order == armor.Order;

        public float OnModifyReduceHealth(float originalAmountToReduceBy) => originalAmountToReduceBy - 5f;
        public float OnModifyReplensihHealth(float originalAmountToReplenish) => originalAmountToReplenish;
        public float OnUpdate(float currentAmount, float maxAmount) => currentAmount;
    }

    public struct Shield : IHealthType
    {
        public int Order => 3;

        public bool Equals(IHealthType other) => other is Shield armor && Order == armor.Order;

        public float OnModifyReduceHealth(float originalAmountToReduceBy) => Mathf.Min(originalAmountToReduceBy * .5f, originalAmountToReduceBy - 15f);
        public float OnModifyReplensihHealth(float originalAmountToReplenish) => originalAmountToReplenish * .75f;
        public float OnUpdate(float currentAmount, float maxAmount) => currentAmount;
    }

    public class RegenerativeShield : IHealthType
    {
        private readonly float _regenSpeed;

        public RegenerativeShield(float regenSpeed) => _regenSpeed = regenSpeed;

        public int Order => 4;

        public bool Equals(IHealthType other) => other is RegenerativeShield shield && shield.Order == Order && shield._regenSpeed == _regenSpeed;
        public float OnModifyReduceHealth(float originalAmountToReduceBy) => Mathf.Min(originalAmountToReduceBy * .5f, originalAmountToReduceBy - 15f);
        public float OnModifyReplensihHealth(float originalAmountToReplenish) => originalAmountToReplenish * .75f;
        public float OnUpdate(float currentAmount, float maxAmount) => Mathf.Min(currentAmount + _regenSpeed, maxAmount);
    }
}
