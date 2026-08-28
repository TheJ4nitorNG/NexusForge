
using Company.Platform.Core;

namespace Company.Platform.UnitTests;

public class OperationContextTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        DateTimeOffset startedAt = DateTimeOffset.UtcNow;
        string product = "TestProduct";
        string component = "TestComponent";

        // Act
        OperationContext context = new(id, startedAt, product, component);

        // Assert
        _ = context.OperationId.Should().Be(id);
        _ = context.StartedAt.Should().Be(startedAt);
        _ = context.Product.Should().Be(product);
        _ = context.Component.Should().Be(component);
    }
}
