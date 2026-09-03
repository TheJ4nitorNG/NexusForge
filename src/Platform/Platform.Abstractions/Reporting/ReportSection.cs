namespace Company.Platform.Abstractions.Reporting;

/// <summary>
/// Represents a structured, tabular section within an exportable report.
/// </summary>
public sealed record ReportSection
{
    /// <summary>Gets the heading of this section.</summary>
    public required string Heading { get; init; }

    /// <summary>Gets the headers of the data columns in this section.</summary>
    public required IReadOnlyList<string> Headers { get; init; }

    /// <summary>Gets the rows of tabular string values in this section.</summary>
    public required IReadOnlyList<IReadOnlyList<string>> Rows { get; init; }
}
