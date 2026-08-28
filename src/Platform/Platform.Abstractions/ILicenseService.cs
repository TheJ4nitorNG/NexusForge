namespace Company.Platform.Abstractions;

/// <summary>
/// Represents the status of a license.
/// </summary>
public enum LicenseStatus
{
    /// <summary>The license is valid.</summary>
    Valid,
    /// <summary>The license has expired.</summary>
    Expired,
    /// <summary>The license is invalid.</summary>
    Invalid
}

/// <summary>
/// Provides mechanisms to check license and entitlement status.
/// </summary>
public interface ILicenseService
{
    /// <summary>
    /// Gets the overall license status asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The license status.</returns>
    Task<LicenseStatus> GetStatusAsync(
        CancellationToken cancellationToken);

    /// <summary>
    /// Checks if a specific entitlement is active.
    /// </summary>
    /// <param name="entitlement">The entitlement identifier.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>True if the entitlement is active; otherwise, false.</returns>
    Task<bool> HasEntitlementAsync(
        string entitlement,
        CancellationToken cancellationToken);
}
