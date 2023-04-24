using FluentNUnity.Shared;
using NUnit.Framework;
using UnityEngine;

using Range = NUnit.Framework.RangeAttribute;

namespace Hybel.HealthSystem.Tests
{
    [Category("Health")]
    public class health_tests
    {
        [Test]
        public void create_health()
        {
            // Act
            IHealth health = new Health();

            // Assert
            health.Should().NotBeNull();
        }

        [Test]
        public void create_health_with_value([Range(-100f, 100f, 25f)] float healthValue)
        {
            // Act
            IHealth health = new Health(healthValue);

            // Assert
            health.Amount.Should().Be(Mathf.Max(healthValue, 0f));
        }

        [Test]
        public void create_health_with_type_and_value([Range(-100f, 100f, 25f)] float healthValue)
        {
            // Arrange
            IHealthType healthType = new MockHealthType();

            // Act
            IHealth health = new Health(healthValue, healthType);

            // Assert
            health.HealthType.Should().Be(healthType);
            health.Amount.Should().Be(Mathf.Max(healthValue, 0f));
        }
    }
}
