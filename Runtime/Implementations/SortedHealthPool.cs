using System.Collections;
using System.Collections.Generic;

namespace Hybel.HealthSystem
{
    public readonly struct SortedHealthPool : IHealthPool
    {
        private readonly Option<List<IHealth>> _healths;
        private readonly IComparer<IHealth> _comparer;

        public SortedHealthPool(List<IHealth> healths)
        {
            _comparer = HealthUtils.DefaultComparer;
            _healths = healths;
            Sort();
        }

        public List<IHealth> Healths => _healths.DangerousValue;
        public IComparer<IHealth> Comparer => _comparer;

        public void Sort() => Healths.Sort(Comparer);

        public IEnumerator<IHealth> GetEnumerator() => Healths.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
