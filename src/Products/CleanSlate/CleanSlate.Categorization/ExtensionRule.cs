namespace Company.CleanSlate.Categorization;

/// <summary>
/// Categorizes files based on their extension.
/// </summary>
public sealed class ExtensionRule : IStorageClassificationRule
{
    /// <inheritdoc />
    public string Id => "cleanslate.rule.extension";

    private static readonly HashSet<string> TempExtensions = new(StringComparer.OrdinalIgnoreCase) { ".tmp", ".temp", ".bak", ".old" };
    private static readonly HashSet<string> InstallerExtensions = new(StringComparer.OrdinalIgnoreCase) { ".msi", ".iso", ".exe" }; // Note: .exe is broad, confidence would be lower

    /// <inheritdoc />
    public ClassificationResult? Evaluate(FileMetadata metadata)
    {
        if (TempExtensions.Contains(metadata.Extension))
        {
            return new ClassificationResult(StorageCategory.Temporary, 80);
        }

        if (InstallerExtensions.Contains(metadata.Extension))
        {
            // If it's an exe, it's only weakly an installer unless it's in a Downloads folder (handled by a different rule)
            int confidence = metadata.Extension.Equals(".exe", StringComparison.OrdinalIgnoreCase) ? 30 : 90;
            return new ClassificationResult(StorageCategory.Installers, confidence);
        }

        return null;
    }
}
