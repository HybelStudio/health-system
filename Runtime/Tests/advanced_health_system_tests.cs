using FluentNUnity.Shared;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Range = NUnit.Framework.RangeAttribute;

namespace Hybel.HealthSystem.Tests
{

    [Category("Health")]
    [Parallelizable(ParallelScope.Children)]
    public class advanced_health_system_tests
    {
        [Test]
        public void create_health_system()
        {
            // Act
            IHealthSystem healthSystem = new AdvancedHealthSystem(new HealthPool());

            // Assert
            healthSystem.Should().NotBeNull();
        }

        [Test]
        public void create_health_system_with_health_pool([Range(0, 3)] int numberOfHealths)
        {
            // Arrange
            List<IHealth> healths = new();

            for (int i = 0; i < numberOfHealths; i++)
                healths.Add(new MockHealth());

            IHealthPool healthPool = new HealthPool(healths);

            // Act
            IAdvancedHealthSystem healthSystem = new AdvancedHealthSystem(healthPool);

            // Assert
            healthSystem.Should().NotBeNull();
            //healthSystem.Pool.Should().Be(healthPool);
            if (numberOfHealths < 2)
            {
                healthSystem.HealthPool.ShouldBeEquivalentTo(healthPool);
            }
            else
            {
                if (healthSystem.HealthPool != null)
                    healthSystem.HealthPool.Healths.Count.Should().Be(1);
            }
        }

        [Test]
        public void create_health_system_with_health_pool_with_values([Range(-100f, 100f, 25f)] float healthValue, [Range(0, 3)] int numberOfHealths)
        {
            // Arrange
            List<IHealth> healths = new();

            for (int i = 0; i < numberOfHealths; i++)
                healths.Add(new Health(healthValue));

            IHealthPool healthPool = new HealthPool(healths);

            // Act
            IAdvancedHealthSystem healthSystem = new AdvancedHealthSystem(healthPool);

            // Assert
            healthSystem.Should().NotBeNull();
            if (numberOfHealths < 2)
            {
                healthSystem.HealthPool.ShouldBeEquivalentTo(healthPool);
            }
            else
            {
                if (healthSystem.HealthPool != null)
                    healthSystem.HealthPool.Healths.Count.Should().Be(1);
            }

            if (healthSystem.HealthPool != null)
                if (healthSystem.HealthPool.Healths != null)
                    Sum(healthSystem.HealthPool.Healths.Select(h => h.Amount).ToArray()).Should().Be(Mathf.Max(healthValue, 0f) * numberOfHealths);
        }

        [Test]
        public void create_health_system_with_ordered_health_pool([Range(0, 3)] int numberOfHealths)
        {
            // Arrange
            List<IHealth> healths = new();

            for (int i = 0; i < numberOfHealths; i++)
                healths.Add(new Health(default, default, i));

            IHealthPool healthPool = new HealthPool(healths);

            // Act
            IAdvancedHealthSystem healthSystem = new AdvancedHealthSystem(healthPool);

            // Assert
            healthSystem.Should().NotBeNull();
            if (numberOfHealths < 2)
            {
                healthSystem.HealthPool.ShouldBeEquivalentTo(healthPool);
            }
            else
            {
                if (healthSystem.HealthPool != null)
                    healthSystem.HealthPool.Healths.Count.Should().Be(1);
            }

            if (healthSystem.HealthPool != null)
                if (healthSystem.HealthPool.Healths != null)
                    healthSystem.HealthPool.Healths.Should().BeInDescendingOrder(h => h.Order);
        }

        [Test]
        public void reduce_health([Range(-100f, 100f, 25f)] float healthValue, [Range(-100f, 100f, 25f)] float damageValue)
        {
            // Arrange
            IHealth health = new Health(healthValue);
            IAdvancedHealthSystem healthSystem = new AdvancedHealthSystem(health);

            // Act
            IHealthDifference difference = healthSystem.ReduceHealthAdvanced(damageValue);

            // Assert
            if (difference.Healths != null)
            {
                var healths = difference.Healths;
                healths.Should().BeInDescendingOrder(h => h.Order);
                healths[0].Should().Be(new Health(Mathf.Min(healthValue, damageValue)));
            }
            else
            {
                difference.Healths.Should().BeNull();
            }

            healthSystem.CurrentHealth.Should().Be(Mathf.Max(Mathf.Max(healthValue, 0f) - Mathf.Max(damageValue, 0f), 0f));
        }

        [Test]
        public void reduce_health_with_health_type([Range(-100f, 100f, 25f)] float healthValue, [Range(-50f, 50f, 25f)] float damageValue)
        {
            // Arrange
            IHealth health = new Health(healthValue, new MockHealthType());
            IAdvancedHealthSystem healthSystem = new AdvancedHealthSystem(health);

            // Act
            IHealthDifference difference = healthSystem.ReduceHealthAdvanced(damageValue, new MockHealthType());

            // Assert
            if (difference.Healths != null)
            {
                var healths = difference.Healths;
                healths.Should().BeInDescendingOrder(h => h.Order);
                healths[0].Should().Be(new Health(Mathf.Min(healthValue, damageValue), new MockHealthType()));
            }
            else
            {
                difference.Healths.Should().BeNull();
            }

            healthSystem.CurrentHealth.Should().Be(Mathf.Max(Mathf.Max(healthValue, 0f) - Mathf.Max(damageValue, 0f), 0f));
        }

        [Test]
        public void reduce_health_by_health_object([Range(-100f, 100f, 25f)] float healthValue, [Range(-50f, 50f, 25f)] float damageValue)
        {
            // Arrange
            IHealth health = new Health(healthValue);
            IAdvancedHealthSystem healthSystem = new AdvancedHealthSystem(health);
            IHealth healthToReduce = new Health(damageValue);

            // Act
            IHealthDifference difference = healthSystem.ReduceHealthAdvanced(healthToReduce);

            // Assert
            if (difference.Healths != null)
            {
                var healths = difference.Healths;
                healths.Should().BeInDescendingOrder(h => h.Order);
                healths[0].Should().Be(new Health(Mathf.Min(healthValue, damageValue)));
            }
            else
            {
                difference.Healths.Should().BeNull();
            }

            healthSystem.CurrentHealth.Should().Be(Mathf.Max(Mathf.Max(healthValue, 0f) - Mathf.Max(damageValue, 0f), 0f));
        }

        [Test]
        public void reduce_health_by_health_object_with_health_type([Range(-100f, 100f, 25f)] float healthValue, [Range(-50f, 50f, 25f)] float damageValue)
        {
            // Arrange
            IHealth health = new Health(healthValue, new MockHealthType());
            IAdvancedHealthSystem healthSystem = new AdvancedHealthSystem(health);
            IHealth healthToReduce = new Health(damageValue, new MockHealthType());

            // Act
            IHealthDifference difference = healthSystem.ReduceHealthAdvanced(healthToReduce);

            // Assert
            if (difference.Healths != null)
            {
                var healths = difference.Healths;
                healths.Should().BeInDescendingOrder(h => h.Order);
                healths[0].Should().Be(new Health(Mathf.Min(healthValue, damageValue), new MockHealthType()));
            }
            else
            {
                difference.Healths.Should().BeNull();
            }

            healthSystem.CurrentHealth.Should().Be(Mathf.Max(Mathf.Max(healthValue, 0f) - Mathf.Max(damageValue, 0f), 0f));
        }

        [Test]
        public void reduce_health_using_health_pool([Range(-100f, 100f, 25f)] float healthValue, [Range(-50f, 50f, 25f)] float damageValue, [Range(0, 2)] int numberOfHealths, [Range(0, 2)] int numberOfHealthsToReduceBy)
        {
            // Arrange
            List<IHealth> healths = new();

            for (int i = 0; i < numberOfHealths; i++)
                healths.Add(new Health(healthValue));

            IHealthPool healthPool = new HealthPool(healths);

            IAdvancedHealthSystem healthSystem = new AdvancedHealthSystem(healthPool);

            List<IHealth> healthsToReduceBy = new List<IHealth>();

            for (int i = 0; i < numberOfHealthsToReduceBy; i++)
                healthsToReduceBy.Add(new Health(damageValue));

            IHealthPool healthPoolToReduceBy = new HealthPool(healthsToReduceBy);

            // Act
            IHealthDifference difference = healthSystem.ReduceHealthAdvanced(healthPoolToReduceBy);

            // Assert
            if (difference.Healths != null)
            {
                difference.Healths.Should().BeInDescendingOrder(h => h.Order);
            }
            else
            {
                difference.Healths.Should().BeNull();
            }

            healthSystem.CurrentHealth.Should().Be(Mathf.Max(Mathf.Max(healthValue * numberOfHealths, 0f) - Mathf.Max(damageValue * numberOfHealthsToReduceBy, 0f), 0f));
        }

        [Test]
        public void replenish_health([Range(-100f, 100f, 25f)] float healthValue, [Range(-100f, 100f, 25f)] float maxHealthValue, [Range(-50f, 50f, 25f)] float replenishValue)
        {
            // Arrange
            IHealth health = new Health(healthValue);
            IHealth maxHealth = new Health(maxHealthValue);
            IAdvancedHealthSystem healthSystem = new AdvancedHealthSystem(health, maxHealth);

            // Act
            IHealthDifference difference = healthSystem.ReplenishHealthAdvanced(replenishValue);

            // Assert
            float clampedMaxHealth = Mathf.Max(maxHealthValue, 0f);
            float clampedHealth = Mathf.Clamp(healthValue, 0f, clampedMaxHealth);
            float highestPossibleReplenishment = Mathf.Max(clampedMaxHealth - clampedHealth, 0f);
            float clampedReplenishment = Mathf.Clamp(replenishValue, 0f, highestPossibleReplenishment);

            if (difference.Healths != null)
            {
                List<IHealth> healths = difference.Healths;
                healths.Should().BeInDescendingOrder(h => h.Order);
                healths[0].Amount.Should().Be(clampedReplenishment);
            }
            else
            {
                difference.Healths.Should().BeNull();
            }

            healthSystem.CurrentHealth.Should().Be(clampedHealth + clampedReplenishment);
        }

        [Test]
        public void replenish_health_with_health_type([Range(-100f, 100f, 25f)] float healthValue, [Range(-100f, 100f, 25f)] float maxHealthValue, [Range(-50f, 50f, 25f)] float replenishValue)
        {
            // Arrange
            IHealth health = new Health(healthValue);
            IHealth maxHealth = new Health(maxHealthValue);
            IAdvancedHealthSystem healthSystem = new AdvancedHealthSystem(health, maxHealth);

            // Act
            IHealthDifference difference = healthSystem.ReplenishHealthAdvanced(replenishValue);

            // Assert
            float clampedMaxHealth = Mathf.Max(maxHealthValue, 0f);
            float clampedHealth = Mathf.Clamp(healthValue, 0f, clampedMaxHealth);
            float highestPossibleReplenishment = Mathf.Max(clampedMaxHealth - clampedHealth, 0f);
            float clampedReplenishment = Mathf.Clamp(replenishValue, 0f, highestPossibleReplenishment);

            if (difference.Healths != null)
            {
                List<IHealth> healths = difference.Healths;
                healths.Should().BeInDescendingOrder(h => h.Order);
                healths[0].Amount.Should().Be(clampedReplenishment);
            }
            else
            {
                difference.Healths.Should().BeNull();
            }

            healthSystem.CurrentHealth.Should().Be(clampedHealth + clampedReplenishment);
        }

        [Test]
        public void replenish_health_by_health_object([Range(-100f, 100f, 25f)] float healthValue, [Range(-100f, 100f, 25f)] float maxHealthValue, [Range(-50f, 50f, 25f)] float replenishValue)
        {
            // Arrange
            IHealth health = new Health(healthValue);
            IHealth maxHealth = new Health(maxHealthValue);
            IAdvancedHealthSystem healthSystem = new AdvancedHealthSystem(health, maxHealth);
            IHealth replenishHealth = new Health(replenishValue);

            // Act
            IHealthDifference difference = healthSystem.ReplenishHealthAdvanced(replenishHealth);

            // Assert
            float clampedMaxHealth = Mathf.Max(maxHealthValue, 0f);
            float clampedHealth = Mathf.Clamp(healthValue, 0f, clampedMaxHealth);
            float highestPossibleReplenishment = Mathf.Max(clampedMaxHealth - clampedHealth, 0f);
            float clampedReplenishment = Mathf.Clamp(replenishValue, 0f, highestPossibleReplenishment);

            if (difference.Healths != null)
            {
                var healths = difference.Healths;
                healths.Should().BeInDescendingOrder(h => h.Order);
                healths[0].Amount.Should().Be(clampedReplenishment);
            }
            else
            {
                difference.Healths.Should().BeNull();
            }

            healthSystem.CurrentHealth.Should().Be(clampedHealth + clampedReplenishment);
        }

        [Test]
        public void replenish_health_using_health_pool([Range(-100f, 100f, 50f)] float healthValue, [Range(-100f, 100f, 50f)] float maxHealthValue, [Range(-50f, 50f, 50f)] float replenishValue, [Range(0, 2)] int numberOfHealths, [Range(0, 2)] int numberOfHealthsToReplenishWith)
        {
            // Arrange
            List<IHealth> healths = new();
            List<IHealth> maxHealths = new();

            for (int i = 0; i < numberOfHealths; i++)
                healths.Add(new Health(healthValue));

            for (int i = 0; i < numberOfHealths; i++)
                maxHealths.Add(new Health(maxHealthValue));

            IHealthPool healthPool = new HealthPool(healths);
            IHealthPool maxHealthPool = new HealthPool(maxHealths);

            IAdvancedHealthSystem healthSystem = new AdvancedHealthSystem(healthPool, maxHealthPool);

            List<IHealth> healthsToReplenish = new();

            for (int i = 0; i < numberOfHealthsToReplenishWith; i++)
                healthsToReplenish.Add(new Health(replenishValue));

            IHealthPool healthPoolToReplenish = new HealthPool(healthsToReplenish);

            // Act
            IHealthDifference difference = healthSystem.ReplenishHealthAdvanced(healthPoolToReplenish);

            // Assert
            float clampedMaxHealth = Mathf.Max(maxHealthValue * numberOfHealths, 0f);
            float clampedHealth = Mathf.Clamp(healthValue * numberOfHealths, 0f, clampedMaxHealth);
            float highestPossibleReplenishment = Mathf.Max(clampedMaxHealth - clampedHealth, 0f);
            float clampedReplenishment = Mathf.Clamp(replenishValue * numberOfHealthsToReplenishWith, 0f, highestPossibleReplenishment);

            if (difference.Healths != null)
            {
                List<IHealth> differenceHealths = difference.Healths;
                differenceHealths.Count.Should().Be(numberOfHealthsToReplenishWith);
                differenceHealths.Should().BeInDescendingOrder(h => h.Order);
                difference.TotalDifference.Should().Be(clampedReplenishment);
            }
            else
            {
                difference.Healths.Should().BeNull();
            }

            healthSystem.CurrentHealth.Should().Be(clampedHealth + clampedReplenishment);
        }

        [Test]
        public void reduce_health_using_health_with_health_type_which_modifies_the_reduced_amount([Range(0f, 50f, 10f)] float healthValue, [Range(0f, 50f, 10f)] float damageValue)
        {
            // Arrange
            IHealth health = new Health(healthValue, new Armor());
            IAdvancedHealthSystem healthSystem = new AdvancedHealthSystem(health);

            // Act
            IHealthDifference difference = healthSystem.ReduceHealthAdvanced(damageValue);

            // Assert
            float highestPossibleDamage = Mathf.Min(Mathf.Max(damageValue - 2f, 0f), healthValue);
            difference.Should().NotBeNull();
            if (difference.Healths != null)
            {
                List<IHealth> healths = difference.Healths;
                healths.Should().BeInDescendingOrder(h => h.Order);
                healths.Count.Should().Be(1);
                healths[0].Amount.Should().Be(highestPossibleDamage);
            }
            else if (healthValue > 0f && damageValue > 0f && healthValue > damageValue)
            {
                Assert.Fail();
            }
            else
            {
                healthSystem.CurrentHealth.Should().Be(Mathf.Max(healthValue - highestPossibleDamage, 0f));
            }
        }

        [Test]
        public void on_update_using_test_health_type_regenerative_shield([Range(0f, 10f, 1f)] float regenSpeed)
        {
            // Arrange
            var healthPool = new HealthPool(new Health(0f, new RegenerativeShield(regenSpeed)));
            var maxHealthPool = new HealthPool(new Health(100f, new RegenerativeShield(regenSpeed)));

            var healthSystem = new AdvancedHealthSystem(healthPool, maxHealthPool);

            // Act
            healthSystem.OnUpdate();

            // Assert
            healthSystem.CurrentHealth.Should().Be(regenSpeed, because: $"it starts at zero health and when OnUpdate is called RegenerativeShield adds health equals to the 'regenSpeed' ({regenSpeed}).");
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
