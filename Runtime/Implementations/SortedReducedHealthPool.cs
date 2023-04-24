using System.Collections;
using System.Collections.Generic;

namespace Hybel.HealthSystem
{
    public readonly struct SortedReducedHealthPool : IHealthDifference
    {
        private readonly Option<List<IHealth>> _healths;
        private readonly IComparer<IHealth> _comparer;

        public SortedReducedHealthPool(List<IHealth> healths)
        {
            _healths = healths;
            _comparer = HealthUtils.DefaultComparer;
            Sort();
        }

        public List<IHealth> Healths => _healths.DangerousValue;
        public IComparer<IHealth> Comparer => _comparer;

        public float TotalDifference
        {
            get
            {
                if (_healths.TryUnwrap(out var healths))
                {
                    float total = 0f;

                    foreach (var health in healths)
                        total += health.Amount;

                    return total;
                }

                return 0f;
            }
        }

        public void Sort() => Healths.Sort(Comparer);

        public IEnumerator<IHealth> GetEnumerator() => Healths.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}