using System.CommandLine;
using Company.Platform.Services;
using Company.SysMedic.Diagnostics;
using Company.SysMedic.Diagnostics.Windows;
using Spectre.Console;

namespace Company.SysMedic.Cli;

/// <summary>
/// The main entry point for the SysMedic CLI application.
/// </summary>
public static class Program
{
    /// <summary>
    /// The main execution method.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>The exit code.</returns>
    public static async Task<int> Main(string[] args)
    {
        RootCommand rootCommand = new("SysMedic - Windows diagnostics and technician toolkit.");

        Command scanCommand = new("scan", "Runs diagnostic checks on the system.");

        scanCommand.SetHandler(RunScanCommandAsync);

        rootCommand.AddCommand(scanCommand);

        return await rootCommand.InvokeAsync(args).ConfigureAwait(false);
    }

    private static async Task RunScanCommandAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]SysMedic Diagnostics[/] - Starting Scan...");
        AnsiConsole.WriteLine();

        // Manual DI setup for MVP simplicity
        ServiceManager serviceManager = new();

        IDiagnosticCheck[] checks =
        [
            new LogicalDiskSpaceCheck(),
            new CriticalServicesCheck(serviceManager)
        ];

        DiagnosticCoordinator coordinator = new(checks);

        DiagnosticContext context = new()
        {
            ScanId = Guid.NewGuid().ToString("N"),
            StartedAt = DateTimeOffset.UtcNow,
            CancellationToken = CancellationToken.None,
            Snapshot = new DummySnapshot() // Real snapshot provider would go here
        };

        ScanReport? report = null;

        await AnsiConsole.Status()
            .StartAsync("Running diagnostic checks...", async ctx =>
            {
                report = await coordinator.RunScanAsync(context, CancellationToken.None).ConfigureAwait(false);
            }).ConfigureAwait(false);

        if (report != null)
        {
            AnsiConsole.MarkupLine($"Scan complete in [yellow]{report.Duration.TotalSeconds:N2}s[/].");

            string healthColor = report.OverallHealthScore > 80 ? "green" : (report.OverallHealthScore > 50 ? "yellow" : "red");
            AnsiConsole.MarkupLine($"Overall Health Score: [{healthColor} bold]{report.OverallHealthScore}/100[/]");
            AnsiConsole.WriteLine();

            Table table = new();
            table.AddColumn("Check");
            table.AddColumn("Status");
            table.AddColumn("Summary");

            foreach (DiagnosticResult result in report.Results)
            {
                string statusMarkup = result.Status switch
                {
                    DiagnosticStatus.Passed => "[green]PASS[/]",
                    DiagnosticStatus.Warning => "[yellow]WARN[/]",
                    DiagnosticStatus.Failed => "[red]FAIL[/]",
                    DiagnosticStatus.Error => "[bold red]ERR[/]",
                    DiagnosticStatus.Skipped => "[grey]SKIP[/]",
                    DiagnosticStatus.NotRun => throw new NotImplementedException(),
                    DiagnosticStatus.Running => throw new NotImplementedException(),
                    DiagnosticStatus.Unknown => throw new NotImplementedException(),
                    _ => "[grey]UNKNOWN[/]"
                };

                table.AddRow(
                    new Markup(result.CheckId),
                    new Markup(statusMarkup),
                    new Markup(result.Summary));
            }

            AnsiConsole.Write(table);

            var issues = report.Results.SelectMany(r => r.Findings).ToList();
            if (issues.Count != 0)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[bold red]Actionable Findings:[/]");
                foreach (DiagnosticFinding finding in issues)
                {
                    AnsiConsole.MarkupLine($"- [{finding.Severity}] {finding.Title}: {finding.Description}");
                }
            }
        }
    }

    private sealed class DummySnapshot : ISystemSnapshot
    {
        public string WindowsVersion => "Windows 11";
        public string BuildNumber => "22621";
        public string Architecture => "x64";
        public string Cpu => "Generic CPU";
        public long TotalRamBytes => 16L * 1024 * 1024 * 1024;
    }
}
