namespace Company.CleanSlate.Cleanup;

/// <summary>
/// Exception thrown when a file deletion attempt targets a critical, blacklisted system path.
/// </summary>
/// <param name="message">The exception message.</param>
public sealed class CriticalSecurityException(string message) : Exception(message)
{
}
