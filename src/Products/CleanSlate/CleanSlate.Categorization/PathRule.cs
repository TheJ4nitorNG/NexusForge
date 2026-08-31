namespace Company.CleanSlate.Categorization;

/// <summary>
/// Categorizes files based on their path structure.
/// </summary>
public sealed class PathRule : IStorageClassificationRule
{
    /// <inheritdoc />
    public string Id => "cleanslate.rule.path";

    /// <inheritdoc />
    public ClassificationResult? Evaluate(FileMetadata metadata)
    {
        return metadata.FilePath.Contains(@"\Windows\", StringComparison.OrdinalIgnoreCase)
            ? (metadata.FilePath.Contains(@"\Temp\", StringComparison.OrdinalIgnoreCase) || metadata.FilePath.Contains(@"\Prefetch\", StringComparison.OrdinalIgnoreCase)
                ? new ClassificationResult(StorageCategory.Temporary, 90)
                : new ClassificationResult(StorageCategory.Windows, 100))
            : metadata.FilePath.Contains(@"\Users\", StringComparison.OrdinalIgnoreCase)
                ? (metadata.FilePath.Contains(@"\AppData\Local\Temp", StringComparison.OrdinalIgnoreCase)
                    ? new ClassificationResult(StorageCategory.Temporary, 95)
                    : (metadata.FilePath.Contains(@"\AppData\", StringComparison.OrdinalIgnoreCase) && metadata.FilePath.Contains(@"\Cache", StringComparison.OrdinalIgnoreCase)
                        ? new ClassificationResult(StorageCategory.Caches, 90)
                        : new ClassificationResult(StorageCategory.User, 80)))
                : null;
    }
}
