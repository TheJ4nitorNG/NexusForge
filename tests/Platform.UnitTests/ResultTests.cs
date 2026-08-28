
using Company.Platform.Core;

namespace Company.Platform.UnitTests;

public class ResultTests
{
    [Fact]
    public void Success_ShouldReturnSuccessResult()
    {
        // Act
        Result result = Result.Success();

        // Assert
        _ = result.IsSuccess.Should().BeTrue();
        _ = result.IsFailure.Should().BeFalse();
        _ = result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Failure_ShouldReturnFailureResult_WithError()
    {
        // Arrange
        Error error = new("Test.Error", "Test error message");

        // Act
        Result result = Result.Failure(error);

        // Assert
        _ = result.IsSuccess.Should().BeFalse();
        _ = result.IsFailure.Should().BeTrue();
        _ = result.Error.Should().Be(error);
    }

    [Fact]
    public void Success_WithValue_ShouldReturnSuccessResult()
    {
        // Act
        Result<string> result = Result.Success("Test Value");

        // Assert
        _ = result.IsSuccess.Should().BeTrue();
        _ = result.Value.Should().Be("Test Value");
        _ = result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Failure_WithValue_ShouldThrowOnValueAccess()
    {
        // Arrange
        Error error = new("Test.Error", "Test error message");
        Result<string> result = Result.Failure<string>(error);

        // Act
        Func<string> action = () => result.Value;

        // Assert
        _ = action.Should().Throw<InvalidOperationException>();
    }
}
