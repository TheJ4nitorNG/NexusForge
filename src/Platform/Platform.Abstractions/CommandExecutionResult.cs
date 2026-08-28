namespace Company.Platform.Abstractions;

/// <summary>
/// Represents the result of a command execution.
/// </summary>
/// <param name="ExitCode">The exit code of the process.</param>
/// <param name="StandardOutput">The standard output.</param>
/// <param name="StandardError">The standard error.</param>
/// <param name="ExecutionDuration">The duration of the execution.</param>
public sealed record CommandExecutionResult(
    int ExitCode,
    string StandardOutput,
    string StandardError,
    TimeSpan ExecutionDuration);
