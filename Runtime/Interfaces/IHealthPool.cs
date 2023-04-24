using System.Collections.Generic;

namespace Hybel.HealthSystem
{
    public interface IHealthPool : IEnumerable<IHealth>
    {
        public List<IHealth> Healths { get; }
        public IComparer<IHealth> Comparer { get; }

        public void Sort();
    }

    public interface IReadonlyHealthPool : IEnumerable<IReadonlyHealth>
    {
        public IReadOnlyList<IReadonlyHealth> Healths { get; }
    }

    public static class IHealthPoolExtensions
    {
        public static void AddHealth(this IHealthPool healthPool, IHealth health)
        {
            if (healthPool?.Healths != null)
                healthPool.Healths.Add(health);
        }

        public static bool RemoveHealth(this IHealthPool healthPool, IHealth health)
        {
            if (healthPool?.Healths != null)
                return healthPool.Healths.Remove(health);

            return false;
        }

        public static IReadonlyHealthPool ToReadOnly(this IHealthPool healthPool)
        {
            if (healthPool is null)
                return null;

            if (healthPool.Healths is null)
                return null;

            List<IReadonlyHealth> listOfHealths = new List<IReadonlyHealth>();

            foreach (var health in healthPool.Healths)
                listOfHealths.Add(new ReadonlyHealth(health));

            return new ReadonlyHealthPool(listOfHealths);
        }
    }
}