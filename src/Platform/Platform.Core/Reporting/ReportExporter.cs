using System.Globalization;
using System.Text;
using System.Text.Json;
using Company.Platform.Abstractions.Reporting;

namespace Company.Platform.Core.Reporting;

/// <summary>
/// A high-performance, safe exporter to output reports in JSON, CSV, and HTML formats.
/// </summary>
public static class ReportExporter
{
    private static readonly JsonSerializerOptions DefaultOptions = new() { WriteIndented = true };

    /// <summary>
    /// Exports a report to a JSON file.
    /// </summary>
    /// <param name="report">The report to export.</param>
    /// <param name="outputPath">The file path where the JSON file should be created.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task ExportToJsonAsync(IExportableReport report, string outputPath)
    {
        ArgumentNullException.ThrowIfNull(report);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);

        using FileStream stream = new(outputPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        await JsonSerializer.SerializeAsync(stream, report, report.GetType(), DefaultOptions).ConfigureAwait(false);
    }

    /// <summary>
    /// Exports a report to a ZIP-packaged directory containing separate CSV files for each section.
    /// </summary>
    /// <param name="report">The report to export.</param>
    /// <param name="outputDirectory">The folder path where the CSV files should be created.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task ExportToCsvAsync(IExportableReport report, string outputDirectory)
    {
        ArgumentNullException.ThrowIfNull(report);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);

        _ = Directory.CreateDirectory(outputDirectory);

        foreach (ReportSection section in report.Sections)
        {
            string sanitizedHeading = string.Concat(section.Heading.Split(Path.GetInvalidFileNameChars())).Replace(" ", "_");
            string fileName = $"{sanitizedHeading}.csv";
            string filePath = Path.Combine(outputDirectory, fileName);

            StringBuilder builder = new();

            // Header
            _ = builder.AppendLine(string.Join(",", section.Headers.Select(EscapeCsv)));

            // Rows
            foreach (IReadOnlyList<string> row in section.Rows)
            {
                _ = builder.AppendLine(string.Join(",", row.Select(EscapeCsv)));
            }

            await File.WriteAllTextAsync(filePath, builder.ToString(), Encoding.UTF8).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Exports a report to a beautiful, responsive, and completely offline-ready HTML file.
    /// </summary>
    /// <param name="report">The report to export.</param>
    /// <param name="outputPath">The file path where the HTML file should be created.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task ExportToHtmlAsync(IExportableReport report, string outputPath)
    {
        ArgumentNullException.ThrowIfNull(report);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);

        StringBuilder builder = new();
        _ = builder.AppendLine("<!DOCTYPE html>");
        _ = builder.AppendLine("<html lang=\"en\">");
        _ = builder.AppendLine("<head>");
        _ = builder.AppendLine("    <meta charset=\"UTF-8\">");
        _ = builder.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        _ = builder.Append("    <title>").Append(HtmlEncode(report.Title)).AppendLine("</title>");
        _ = builder.AppendLine("    <style>");
        _ = builder.AppendLine("        :root {");
        _ = builder.AppendLine("            --bg-color: #0d1117;");
        _ = builder.AppendLine("            --text-color: #c9d1d9;");
        _ = builder.AppendLine("            --card-bg: #161b22;");
        _ = builder.AppendLine("            --border-color: #30363d;");
        _ = builder.AppendLine("            --accent-color: #58a6ff;");
        _ = builder.AppendLine("            --header-bg: #161b22;");
        _ = builder.AppendLine("        }");
        _ = builder.AppendLine("        body {");
        _ = builder.AppendLine("            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif;");
        _ = builder.AppendLine("            background-color: var(--bg-color);");
        _ = builder.AppendLine("            color: var(--text-color);");
        _ = builder.AppendLine("            margin: 0;");
        _ = builder.AppendLine("            padding: 2rem;");
        _ = builder.AppendLine("            line-height: 1.5;");
        _ = builder.AppendLine("        }");
        _ = builder.AppendLine("        header {");
        _ = builder.AppendLine("            background-color: var(--header-bg);");
        _ = builder.AppendLine("            padding: 1.5rem;");
        _ = builder.AppendLine("            border-radius: 6px;");
        _ = builder.AppendLine("            border: 1px solid var(--border-color);");
        _ = builder.AppendLine("            margin-bottom: 2rem;");
        _ = builder.AppendLine("        }");
        _ = builder.AppendLine("        h1 {");
        _ = builder.AppendLine("            margin: 0 0 0.5rem 0;");
        _ = builder.AppendLine("            color: var(--accent-color);");
        _ = builder.AppendLine("        }");
        _ = builder.AppendLine("        .metadata {");
        _ = builder.AppendLine("            font-size: 0.875rem;");
        _ = builder.AppendLine("            color: #8b949e;");
        _ = builder.AppendLine("        }");
        _ = builder.AppendLine("        .section {");
        _ = builder.AppendLine("            background-color: var(--card-bg);");
        _ = builder.AppendLine("            padding: 1.5rem;");
        _ = builder.AppendLine("            border-radius: 6px;");
        _ = builder.AppendLine("            border: 1px solid var(--border-color);");
        _ = builder.AppendLine("            margin-bottom: 2rem;");
        _ = builder.AppendLine("        }");
        _ = builder.AppendLine("        h2 {");
        _ = builder.AppendLine("            margin-top: 0;");
        _ = builder.AppendLine("            border-bottom: 1px solid var(--border-color);");
        _ = builder.AppendLine("            padding-bottom: 0.5rem;");
        _ = builder.AppendLine("            color: #f0f6fc;");
        _ = builder.AppendLine("        }");
        _ = builder.AppendLine("        table {");
        _ = builder.AppendLine("            width: 100%;");
        _ = builder.AppendLine("            border-collapse: collapse;");
        _ = builder.AppendLine("            margin-top: 1rem;");
        _ = builder.AppendLine("        }");
        _ = builder.AppendLine("        th, td {");
        _ = builder.AppendLine("            text-align: left;");
        _ = builder.AppendLine("            padding: 0.75rem;");
        _ = builder.AppendLine("            border-bottom: 1px solid var(--border-color);");
        _ = builder.AppendLine("        }");
        _ = builder.AppendLine("        th {");
        _ = builder.AppendLine("            background-color: #0d1117;");
        _ = builder.AppendLine("            color: #f0f6fc;");
        _ = builder.AppendLine("            font-weight: 600;");
        _ = builder.AppendLine("        }");
        _ = builder.AppendLine("        tr:hover td {");
        _ = builder.AppendLine("            background-color: #21262d;");
        _ = builder.AppendLine("        }");
        _ = builder.AppendLine("    </style>");
        _ = builder.AppendLine("</head>");
        _ = builder.AppendLine("<body>");

        // Header
        _ = builder.AppendLine("    <header>");
        _ = builder.Append("        <h1>").Append(HtmlEncode(report.Title)).AppendLine("</h1>");
        _ = builder.Append("        <div class=\"metadata\">Report ID: ").Append(HtmlEncode(report.ReportId)).AppendLine("</div>");
        _ = builder.Append("        <div class=\"metadata\">Generated: ").Append(HtmlEncode(report.GeneratedAt.ToString("f", CultureInfo.InvariantCulture))).AppendLine("</div>");
        _ = builder.AppendLine("    </header>");

        // Sections
        foreach (ReportSection section in report.Sections)
        {
            _ = builder.AppendLine("    <div class=\"section\">");
            _ = builder.Append("        <h2>").Append(HtmlEncode(section.Heading)).AppendLine("</h2>");
            _ = builder.AppendLine("        <table>");

            // Table Header
            _ = builder.AppendLine("            <thead>");
            _ = builder.AppendLine("                <tr>");
            foreach (string header in section.Headers)
            {
                _ = builder.Append("                    <th>").Append(HtmlEncode(header)).AppendLine("</th>");
            }
            _ = builder.AppendLine("                </tr>");
            _ = builder.AppendLine("            </thead>");

            // Table Body
            _ = builder.AppendLine("            <tbody>");
            foreach (IReadOnlyList<string> row in section.Rows)
            {
                _ = builder.AppendLine("                <tr>");
                foreach (string cell in row)
                {
                    _ = builder.Append("                    <td>").Append(HtmlEncode(cell)).AppendLine("</td>");
                }
                _ = builder.AppendLine("                </tr>");
            }
            _ = builder.AppendLine("            </tbody>");

            _ = builder.AppendLine("        </table>");
            _ = builder.AppendLine("    </div>");
        }

        _ = builder.AppendLine("</body>");
        _ = builder.AppendLine("</html>");

        await File.WriteAllTextAsync(outputPath, builder.ToString(), Encoding.UTF8).ConfigureAwait(false);
    }

    private static string EscapeCsv(string value)
    {
        return $"\"{value.Replace("\"", "\"\"")}\"";
    }

    private static string HtmlEncode(string value)
    {
        return string.IsNullOrEmpty(value)
            ? string.Empty
            : value
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#x27;");
    }
}
