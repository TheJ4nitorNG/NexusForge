namespace Company.Platform.Abstractions;

/// <summary>
/// Represents a request to execute a command.
/// </summary>
/// <param name="Command">The command to execute.</param>
/// <param name="Arguments">The arguments for the command.</param>
/// <param name="Policy">The execution policy.</param>
/// <param name="RequiresElevation">Whether the command requires elevation.</param>
public sealed record CommandRequest(
    string Command,
    IReadOnlyList<string> Arguments,
    ExecutionPolicy Policy,
    bool RequiresElevation);
