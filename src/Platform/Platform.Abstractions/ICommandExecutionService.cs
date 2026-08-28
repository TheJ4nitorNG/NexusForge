namespace Company.Platform.Abstractions;

/// <summary>
/// Provides a mechanism to execute system commands securely.
/// </summary>
public interface ICommandExecutionService
{
    /// <summary>
    /// Executes a command asynchronously.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The result of the command execution.</returns>
    Task<CommandExecutionResult> ExecuteAsync(
        CommandRequest request,
        CancellationToken cancellationToken);
}
