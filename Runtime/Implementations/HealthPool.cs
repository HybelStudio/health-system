using System.Collections;
using System.Collections.Generic;

namespace Hybel.HealthSystem
{
    public readonly struct HealthPool : IHealthPool
    {
        private readonly Option<List<IHealth>> _healths;
        private readonly IComparer<IHealth> _comparer;

        public HealthPool(float amount) : this(new List<IHealth> { new Health(amount) }) { }
        public HealthPool(IHealth health) : this(health != null ? new List<IHealth> { health } : null) { }
        public HealthPool(List<IHealth> healths)
        {
            _healths = healths;
            _comparer = HealthUtils.DefaultComparer;
        }

        public List<IHealth> Healths => _healths.DangerousValue;
        public IComparer<IHealth> Comparer => _comparer;

        public void Sort() => Healths.Sort(Comparer);

        public IEnumerator<IHealth> GetEnumerator() => Healths?.GetEnumerator() ?? new List<IHealth>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public readonly struct ReadonlyHealthPool : IReadonlyHealthPool
    {
        private readonly Option<IReadOnlyList<IReadonlyHealth>> _healths;

        public ReadonlyHealthPool(IReadonlyHealth health) : this(health != null ? new List<IReadonlyHealth> { health } : null) { }
        public ReadonlyHealthPool(List<IReadonlyHealth> healths) => _healths = healths.AsReadOnly();

        public IReadOnlyList<IReadonlyHealth> Healths => _healths.DangerousValue;

        public IEnumerator<IReadonlyHealth> GetEnumerator() => Healths?.GetEnumerator() ?? new List<IReadonlyHealth>().AsReadOnly().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
