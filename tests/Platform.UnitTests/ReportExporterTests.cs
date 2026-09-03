using Company.Platform.Abstractions.Reporting;
using Company.Platform.Core.Reporting;

namespace Company.Platform.UnitTests;

public class ReportExporterTests : IDisposable
{
    private readonly string _tempDir;

    public ReportExporterTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), "Platform_ReportTests_" + Guid.NewGuid().ToString("N"));
        _ = Directory.CreateDirectory(_tempDir);
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(_tempDir))
            {
                Directory.Delete(_tempDir, true);
            }
        }
        catch
        {
            // Ignore clean-up issues
        }
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task ExportToJsonAsync_WritesValidIndentedJson()
    {
        // Arrange
        TestReport report = new()
        {
            ReportId = "rep-123",
            Title = "Disk Report",
            GeneratedAt = DateTimeOffset.UtcNow,
            Sections =
            [
                new ReportSection
                {
                    Heading = "Disk Usage",
                    Headers = ["Drive", "Total"],
                    Rows = [["C:\\", "100GB"], ["D:\\", "50GB"]]
                }
            ]
        };

        string outputPath = Path.Combine(_tempDir, "report.json");

        // Act
        await ReportExporter.ExportToJsonAsync(report, outputPath);

        // Assert
        File.Exists(outputPath).Should().BeTrue();
        string json = await File.ReadAllTextAsync(outputPath);
        json.Should().Contain("\"rep-123\"");
        json.Should().Contain("\"Disk Report\"");
        json.Should().Contain("\"Disk Usage\"");
    }

    [Fact]
    public async Task ExportToCsvAsync_WritesSeparatedCsvFilesWithEscapedValues()
    {
        // Arrange
        TestReport report = new()
        {
            ReportId = "rep-456",
            Title = "Deduplication Summary",
            GeneratedAt = DateTimeOffset.UtcNow,
            Sections =
            [
                new ReportSection
                {
                    Heading = "Duplicate Files",
                    Headers = ["File Name", "Size"],
                    Rows = [["file\"quote\".txt", "10MB"], ["normal.log", "5MB"]]
                }
            ]
        };

        string outputDir = Path.Combine(_tempDir, "csv_output");

        // Act
        await ReportExporter.ExportToCsvAsync(report, outputDir);

        // Assert
        Directory.Exists(outputDir).Should().BeTrue();
        string csvPath = Path.Combine(outputDir, "Duplicate_Files.csv");
        File.Exists(csvPath).Should().BeTrue();

        string csvContent = await File.ReadAllTextAsync(csvPath);
        // Escaped: "file""quote"".txt"
        csvContent.Should().Contain("\"file\"\"quote\"\".txt\"");
    }

    [Fact]
    public async Task ExportToHtmlAsync_WritesResponsiveStandaloneHtml()
    {
        // Arrange
        TestReport report = new()
        {
            ReportId = "rep-789",
            Title = "Technician Health Check",
            GeneratedAt = DateTimeOffset.UtcNow,
            Sections =
            [
                new ReportSection
                {
                    Heading = "Services Status",
                    Headers = ["Service", "Status"],
                    Rows = [["Winmgmt", "Running"], ["EventLog", "Running"]]
                }
            ]
        };

        string outputPath = Path.Combine(_tempDir, "report.html");

        // Act
        await ReportExporter.ExportToHtmlAsync(report, outputPath);

        // Assert
        File.Exists(outputPath).Should().BeTrue();
        string html = await File.ReadAllTextAsync(outputPath);
        html.Should().Contain("<!DOCTYPE html>");
        html.Should().Contain("Technician Health Check");
        html.Should().Contain("Services Status");
        html.Should().Contain("Winmgmt");
    }

    private sealed class TestReport : IExportableReport
    {
        public required string ReportId { get; init; }
        public required string Title { get; init; }
        public required DateTimeOffset GeneratedAt { get; init; }
        public required IReadOnlyList<ReportSection> Sections { get; init; }
    }
}
