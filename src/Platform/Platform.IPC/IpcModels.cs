namespace Company.Platform.IPC;

/// <summary>
/// Represents a generic request over IPC.
/// </summary>
/// <param name="RequestId">The unique identifier for the request.</param>
/// <param name="Operation">The operation to perform.</param>
/// <param name="Payload">The JSON-serialized payload.</param>
public sealed record IpcRequest(
    string RequestId,
    string Operation,
    string? Payload);

/// <summary>
/// Represents a generic response over IPC.
/// </summary>
/// <param name="RequestId">The unique identifier for the request.</param>
/// <param name="IsSuccess">Whether the operation succeeded.</param>
/// <param name="Payload">The JSON-serialized payload or error details.</param>
public sealed record IpcResponse(
    string RequestId,
    bool IsSuccess,
    string? Payload);
