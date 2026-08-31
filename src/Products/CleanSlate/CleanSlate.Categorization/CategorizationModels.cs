namespace Company.CleanSlate.Categorization;

/// <summary>
/// Specifies the classification categories for storage items.
/// </summary>
public enum StorageCategory
{
    /// <summary>Unknown category.</summary>
    Unknown,
    /// <summary>Temporary files.</summary>
    Temporary,
    /// <summary>Cache files.</summary>
    Caches,
    /// <summary>Installer files.</summary>
    Installers,
    /// <summary>User data and documents.</summary>
    User,
    /// <summary>Windows system files.</summary>
    Windows
}

/// <summary>
/// Provides metadata about a file for categorization.
/// </summary>
/// <param name="FilePath">The full path to the file.</param>
/// <param name="Extension">The file extension.</param>
/// <param name="SizeBytes">The size of the file in bytes.</param>
public sealed record FileMetadata(
    string FilePath,
    string Extension,
    long SizeBytes);

/// <summary>
/// Represents the result of a classification rule evaluation.
/// </summary>
/// <param name="Category">The determined category.</param>
/// <param name="Confidence">The confidence level of the determination (0-100).</param>
public sealed record ClassificationResult(
    StorageCategory Category,
    int Confidence);

/// <summary>
/// Defines a rule for categorizing storage items.
/// </summary>
public interface IStorageClassificationRule
{
    /// <summary>
    /// Gets the unique identifier for the rule.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Evaluates the metadata and returns a classification result if applicable.
    /// </summary>
    /// <param name="metadata">The file metadata.</param>
    /// <returns>A classification result, or null if the rule does not apply.</returns>
    ClassificationResult? Evaluate(FileMetadata metadata);
}
