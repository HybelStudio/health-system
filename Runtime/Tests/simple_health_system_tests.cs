using FluentNUnity.Shared;
using NUnit.Framework;
using UnityEngine;

using Range = NUnit.Framework.RangeAttribute;

namespace Hybel.HealthSystem.Tests
{
    [Category("Health")]
    [Parallelizable(ParallelScope.Children)]
    public class simple_health_system_tests
    {
        [Test]
        public void create_health_system([Range(-100f, 100f, 25f)] float healthValue, [Range(-100f, 100f, 25f)] float maxHealthValue)
        {
            IHealthSystem healthSystem = new SimpleHealthSystem(healthValue, maxHealthValue);

            healthSystem.Should().NotBeNull();
            healthSystem.MaxHealth.Should().Be(Mathf.Max(maxHealthValue, 0f));
            healthSystem.CurrentHealth.Should().Be(Mathf.Clamp(healthValue, 0f, Mathf.Max(maxHealthValue, 0f)));
        }

        [Test]
        public void reduce_health([Range(-100f, 100f, 25f)] float healthValue, [Range(-100f, 100f, 25f)] float maxHealthValue, [Range(-100f, 100f, 25f)] float damageValue)
        {
            // Arrange
            IHealthSystem healthSystem = new SimpleHealthSystem(healthValue, maxHealthValue);

            // Act
            float difference = healthSystem.ReduceHealth(damageValue);

            // Assert
            float clampedMaxHealth = Mathf.Max(maxHealthValue, 0f);
            float clampedHealth = Mathf.Clamp(healthValue, 0f, clampedMaxHealth);
            float clampedDamage = Mathf.Max(damageValue, 0f);
            float damageDealt = Mathf.Min(clampedDamage, clampedHealth);

            difference.Should().Be(damageDealt);
            healthSystem.MaxHealth.Should().Be(clampedMaxHealth);
            healthSystem.CurrentHealth.Should().Be(clampedHealth - damageDealt);
        }

        [Test]
        public void replenish_health([Range(-100f, 100f, 25f)] float healthValue, [Range(-100f, 100f, 25f)] float maxHealthValue, [Range(-100f, 100f, 25f)] float replenishValue)
        {
            // Arrange
            IHealthSystem healthSystem = new SimpleHealthSystem(healthValue, maxHealthValue);

            // Act
            float difference = healthSystem.ReplenishHealth(replenishValue);

            // Assert
            float clampedMaxHealth = Mathf.Max(maxHealthValue, 0f);
            float clampedHealth = Mathf.Clamp(healthValue, 0f, clampedMaxHealth);
            float highestAllowedReplenishment = clampedMaxHealth - clampedHealth;
            float clampedReplenishment = Mathf.Clamp(replenishValue, 0f, highestAllowedReplenishment);

            difference.Should().Be(clampedReplenishment);
            healthSystem.MaxHealth.Should().Be(clampedMaxHealth);
            healthSystem.CurrentHealth.Should().Be(clampedHealth + clampedReplenishment);
        }
    }
}
