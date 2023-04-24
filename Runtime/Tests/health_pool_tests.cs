using FluentNUnity.Shared;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Range = NUnit.Framework.RangeAttribute;

namespace Hybel.HealthSystem.Tests
{
    [Category("Health")]
    public class health_pool_tests
    {
        [Test]
        public void create_health_pool()
        {
            // Act
            IHealthPool healthPool = new HealthPool();

            // Assert
            healthPool.Should().NotBeNull();
        }

        [Test]
        public void create_health_pool_with_healths([Range(0, 3)] int numberOfHealths)
        {
            // Arrange
            List<IHealth> healths = new List<IHealth>();

            for (int i = 0; i < numberOfHealths; i++)
                healths.Add(new MockHealth());

            // Act
            IHealthPool healthPool = new HealthPool(healths);

            // Assert
            healthPool.Should().NotBeNull();
            healthPool.Healths.Should().BeSameAs(healths);
        }

        [Test]
        public void create_health_pool_with_healths_with_values([Range(-100f, 100f, 25f)] float healthValue, [Range(0, 3)] int numberOfHealths)
        {
            // Arrange
            List<IHealth> healths = new List<IHealth>();

            for (int i = 0; i < numberOfHealths; i++)
                healths.Add(new Health(healthValue));

            // Act
            IHealthPool healthPool = new HealthPool(healths);

            // Assert
            healthPool.Should().NotBeNull();
            healthPool.Healths.Should().BeSameAs(healths);

            if (healthPool.Healths != null)
                Sum(healthPool.Healths.Select(h => h.Amount).ToArray()).Should().Be(Mathf.Max(healthValue, 0f) * numberOfHealths);
        }

        [Test]
        public void create_health_pool_with_ordered_healths([Range(0, 3)] int numberOfHealths)
        {
            // Arrange
            List<IHealth> healths = new List<IHealth>();

            for (int i = 0; i < numberOfHealths; i++)
                healths.Add(new Health(default, default, i));

            // Act
            IHealthPool healthPool = new HealthPool(healths);

            // Assert
            healthPool.Should().NotBeNull();
            healthPool.Healths.Should().BeSameAs(healths);
        }

        private float Sum(params float[] values)
        {
            float sum = 0f;

            for (int i = 0; i < values.Length; i++)
                sum += values[i];

            return sum;
        }
    }
}
