namespace Hybel.HealthSystem.Tests
{
    internal readonly struct MockHealthType : IHealthType
    {
        public int Order { get; }

        public bool Equals(IHealthType other) => other is MockHealthType mock && Order == mock.Order;

        public float OnModifyReduceHealth(float originalAmountToReduceBy) => 0f;
        public float OnModifyReplensihHealth(float originalAmountToReplenish) => 0f;
        public float OnUpdate(float currentAmount, float maxAmount) => currentAmount;
    }
}
