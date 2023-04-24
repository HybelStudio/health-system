using System.Collections;
using System.Collections.Generic;

namespace Hybel.HealthSystem.Tests
{
    internal readonly struct MockHealthPool : IHealthPool
    {
        public List<IHealth> Healths { get; }
        public IComparer<IHealth> Comparer { get; }

        public void Sort() { }
        public IEnumerator<IHealth> GetEnumerator() => new List<IHealth>().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
