using System.Collections.Generic;

namespace Hybel.HealthSystem
{
    public interface IHealth
    {
        public float Amount { get; set; }
        public int Order { get; }
        public IHealthType HealthType { get; }

        public class ByOrderComparer : IComparer<IHealth>
        {
            public int Compare(IHealth x, IHealth y)
            {
                if (x.Order > y.Order)
                    return -1;

                if (x.Order < y.Order)
                    return 1;

                return 0;
            }
        }
    }

    public interface IReadonlyHealth
    {
        public float Amount { get; }
        public int Order { get; }
        public IHealthType HealthType { get; }
    }

    public static class HealthUtils
    {
        private static bool _usingCustomComparer;
        private static IComparer<IHealth> _customComparer;

        public static readonly IComparer<IHealth> DefaultComparer = new IHealth.ByOrderComparer();

        public static IComparer<IHealth> Comparer
        {
            get => _usingCustomComparer ? _customComparer : DefaultComparer;
            set
            {
                _usingCustomComparer = true;
                _customComparer = value;
            }
        }
    }

    public static class HealthExtensions
    {
        public static void Deconstruct(this IHealth health, out float amount, out int order, out IHealthType healthType)
        {
            amount = health.Amount;
            order = health.Order;
            healthType = health.HealthType;
        }

        public static IReadonlyHealth ToReadonly(this IHealth health) => new ReadonlyHealth(health);
    }
}
