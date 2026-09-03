namespace Company.Platform.Abstractions.Reporting;

/// <summary>
/// Defines the contract for any document or diagnostic summary that can be exported.
/// </summary>
public interface IExportableReport
{
    /// <summary>Gets the unique identifier of the report.</summary>
    string ReportId { get; }

    /// <summary>Gets the human-readable title of the report.</summary>
    string Title { get; }

    /// <summary>Gets the timestamp of when the report was generated.</summary>
    DateTimeOffset GeneratedAt { get; }

    /// <summary>Gets the list of structured tabular sections in the report.</summary>
    IReadOnlyList<ReportSection> Sections { get; }
}
