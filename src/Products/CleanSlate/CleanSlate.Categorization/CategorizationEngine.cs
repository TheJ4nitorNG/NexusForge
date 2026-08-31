namespace Company.CleanSlate.Categorization;

/// <summary>
/// Engine responsible for evaluating a collection of rules against file metadata to determine the best category.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CategorizationEngine"/> class.
/// </remarks>
/// <param name="rules">The rules to evaluate.</param>
public sealed class CategorizationEngine(IEnumerable<IStorageClassificationRule> rules)
{
    private readonly IReadOnlyList<IStorageClassificationRule> _rules = [.. rules];

    /// <summary>
    /// Classifies the given file metadata into a storage category.
    /// </summary>
    /// <param name="metadata">The file metadata.</param>
    /// <returns>The most confident classification result.</returns>
    public ClassificationResult Classify(FileMetadata metadata)
    {
        ClassificationResult bestResult = new(StorageCategory.Unknown, 0);

        foreach (IStorageClassificationRule rule in _rules)
        {
            ClassificationResult? result = rule.Evaluate(metadata);
            if (result != null && result.Confidence > bestResult.Confidence)
            {
                bestResult = result;
            }
        }

        return bestResult;
    }
}
