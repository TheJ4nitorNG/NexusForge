namespace Company.Platform.Abstractions;

/// <summary>
/// Provides secure storage for sensitive data.
/// </summary>
public interface ISecretStore
{
    /// <summary>
    /// Sets a secret value asynchronously.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value to store.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetAsync(string key, ReadOnlyMemory<byte> value);

    /// <summary>
    /// Gets a secret value asynchronously.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>The secret value, or null if not found.</returns>
    Task<ReadOnlyMemory<byte>?> GetAsync(string key);

    /// <summary>
    /// Deletes a secret asynchronously.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(string key);
}
