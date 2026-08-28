namespace Company.Platform.Abstractions;

/// <summary>
/// Provides overall system information.
/// </summary>
public interface ISystemInformationProvider
{
    /// <summary>
    /// Gets system information asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The system information.</returns>
    Task<SystemInformation> GetSystemInformationAsync(
        CancellationToken cancellationToken);
}
