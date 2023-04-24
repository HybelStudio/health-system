using System;
using UnityEngine;

namespace Hybel.HealthSystem
{
    public struct Health : IHealth, IEquatable<Health>
    {
        private float _amount;
        private readonly int _order;
        private readonly Option<IHealthType> _healthType;

        public Health(float amount) : this(amount, null) { }
        public Health(float amount, IHealthType healthType) : this(amount, healthType, healthType != null ? healthType.Order : 0) { }

        /// <summary>
        /// Passing in the order doesn't work the intended way yet. Its currently used for unit testing the sort functionality.
        /// </summary>
        //// TODO: Fix the order input here and make this public.
        internal Health(float amount, IHealthType healthType, int order)
        {
            _amount = Mathf.Max(amount, 0f);
            _order = order;
            _healthType = healthType.Some();
        }

        public float Amount { get => _amount; set => _amount = value; }
        public int Order => _order;
        public IHealthType HealthType => _healthType.DangerousValue;

        public override bool Equals(object obj) => obj is Health health && Equals(health);
        public bool Equals(Health other) => _amount == other._amount && _order == other._order && _healthType.Equals(other._healthType);

        public override int GetHashCode()
        {
            var hashCode = -1571164212;
            hashCode = hashCode * -1521134295 + _amount.GetHashCode();
            hashCode = hashCode * -1521134295 + _order.GetHashCode();
            hashCode = hashCode * -1521134295 + _healthType.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Health left, Health right) => left.Equals(right);
        public static bool operator !=(Health left, Health right) => !(left == right);
    }

    public readonly struct ReadonlyHealth : IReadonlyHealth, IEquatable<ReadonlyHealth>
    {
        private readonly float _amount;
        private readonly int _order;
        private readonly Option<IHealthType> _healthType;

        public ReadonlyHealth(float amount) : this(amount, null) { }
        public ReadonlyHealth(float amount, IHealthType healthType) : this(amount, healthType, healthType != null ? healthType.Order : 0) { }
        public ReadonlyHealth(IHealth health) : this(health.Amount, health.HealthType, health.Order) { }

        /// <summary>
        /// Passing in the order doesn't work the intended way yet. Its currently used for unit testing the sort functionality.
        /// </summary>
        //// TODO: Fix the order input here and make this public.
        internal ReadonlyHealth(float amount, IHealthType healthType, int order)
        {
            _amount = Mathf.Max(amount, 0f);
            _order = order;
            _healthType = healthType.Some();
        }

        public float Amount => _amount;
        public int Order => _order;
        public IHealthType HealthType => _healthType.DangerousValue;

        public override bool Equals(object obj) => obj is ReadonlyHealth health && Equals(health);
        public bool Equals(ReadonlyHealth other) => _amount == other._amount && _order == other._order && _healthType.Equals(other._healthType);

        public override int GetHashCode()
        {
            var hashCode = -1571164212;
            hashCode = hashCode * -1521134295 + _amount.GetHashCode();
            hashCode = hashCode * -1521134295 + _order.GetHashCode();
            hashCode = hashCode * -1521134295 + _healthType.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ReadonlyHealth left, ReadonlyHealth right) => left.Equals(right);
        public static bool operator !=(ReadonlyHealth left, ReadonlyHealth right) => !(left == right);
    }
}
