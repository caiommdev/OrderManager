using WebApplication1.Domain.Services.ShippingCalculators;
using WebApplication1.Domain.ValueObjects;
using Xunit;

namespace OrderManager.Tests.Domain.Services
{
    public class ShippingCalculatorTests
    {
        public class ExpressShippingCalculatorTests
        {
            [Fact]
            public void CalculateShippingCost_ShouldReturnCorrectValue()
            {
                // Arrange
                var calculator = new ExpressShippingCalculator();
                var weight = new Weight(5.0);
                var expectedCost = 5.0m * 1.5m + 10m; // 17.5

                // Act
                var result = calculator.CalculateShippingCost(weight);

                // Assert
                Assert.Equal(expectedCost, result);
            }

            [Fact]
            public void IsEligibleForFreeShipping_ShouldAlwaysReturnFalse()
            {
                // Arrange
                var calculator = new ExpressShippingCalculator();
                var weight = new Weight(1.0);

                // Act
                var result = calculator.IsEligibleForFreeShipping(weight);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void GetShippingTypeName_ShouldReturnExpresso()
            {
                // Arrange
                var calculator = new ExpressShippingCalculator();

                // Act
                var result = calculator.GetShippingTypeName();

                // Assert
                Assert.Equal("Expresso", result);
            }
        }

        public class StandardShippingCalculatorTests
        {
            [Fact]
            public void CalculateShippingCost_ShouldReturnCorrectValue()
            {
                // Arrange
                var calculator = new StandardShippingCalculator();
                var weight = new Weight(10.0);
                var expectedCost = 10.0m * 1.2m; // 12.0

                // Act
                var result = calculator.CalculateShippingCost(weight);

                // Assert
                Assert.Equal(expectedCost, result);
            }

            [Fact]
            public void IsEligibleForFreeShipping_ShouldAlwaysReturnFalse()
            {
                // Arrange
                var calculator = new StandardShippingCalculator();
                var weight = new Weight(1.0);

                // Act
                var result = calculator.IsEligibleForFreeShipping(weight);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void GetShippingTypeName_ShouldReturnPadrao()
            {
                // Arrange
                var calculator = new StandardShippingCalculator();

                // Act
                var result = calculator.GetShippingTypeName();

                // Assert
                Assert.Equal("Padrão", result);
            }
        }

        public class EconomyShippingCalculatorTests
        {
            [Fact]
            public void CalculateShippingCost_WeightAboveLimit_ShouldReturnCorrectValue()
            {
                // Arrange
                var calculator = new EconomyShippingCalculator();
                var weight = new Weight(10.0);
                var expectedCost = 10.0m * 1.1m - 5m; // 6.0

                // Act
                var result = calculator.CalculateShippingCost(weight);

                // Assert
                Assert.Equal(expectedCost, result);
            }

            [Fact]
            public void CalculateShippingCost_WeightBelowFreeShippingLimit_ShouldReturnZero()
            {
                // Arrange
                var calculator = new EconomyShippingCalculator();
                var weight = new Weight(1.5);

                // Act
                var result = calculator.CalculateShippingCost(weight);

                // Assert
                Assert.Equal(0m, result);
            }

            [Fact]
            public void IsEligibleForFreeShipping_WeightBelowLimit_ShouldReturnTrue()
            {
                // Arrange
                var calculator = new EconomyShippingCalculator();
                var weight = new Weight(1.5);

                // Act
                var result = calculator.IsEligibleForFreeShipping(weight);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void IsEligibleForFreeShipping_WeightAboveLimit_ShouldReturnFalse()
            {
                // Arrange
                var calculator = new EconomyShippingCalculator();
                var weight = new Weight(3.0);

                // Act
                var result = calculator.IsEligibleForFreeShipping(weight);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void GetShippingTypeName_ShouldReturnEconomico()
            {
                // Arrange
                var calculator = new EconomyShippingCalculator();

                // Act
                var result = calculator.GetShippingTypeName();

                // Assert
                Assert.Equal("Econômico", result);
            }
        }
    }
}
