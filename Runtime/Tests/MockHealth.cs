namespace Hybel.HealthSystem.Tests
{
    internal struct MockHealth : IHealth
    {
        public float Amount { get; set; }
        public IHealthType HealthType { get; }
        public int Order { get; }
    }
}
