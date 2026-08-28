namespace Company.Platform.Core;

/// <summary>
/// Represents the context of a running operation.
/// </summary>
/// <param name="OperationId">The unique operation identifier.</param>
/// <param name="StartedAt">The timestamp when the operation started.</param>
/// <param name="Product">The name of the product executing the operation.</param>
/// <param name="Component">The component executing the operation.</param>
public sealed record OperationContext(
    Guid OperationId,
    DateTimeOffset StartedAt,
    string Product,
    string Component);
