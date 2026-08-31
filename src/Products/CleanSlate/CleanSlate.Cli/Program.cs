using System.CommandLine;
using Company.CleanSlate.Categorization;
using Company.CleanSlate.Scanner;
using Spectre.Console;

namespace Company.CleanSlate.Cli;

/// <summary>
/// The main entry point for the CleanSlate CLI application.
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
        RootCommand rootCommand = new("CleanSlate - Intelligent storage analysis and cleanup.");

        Command scanCommand = new("scan", "Scans a directory and categorizes its contents.");
        Argument<string> pathArgument = new("path", "The directory path to scan.");
        scanCommand.AddArgument(pathArgument);

        scanCommand.SetHandler(RunScanCommandAsync, pathArgument);

        rootCommand.AddCommand(scanCommand);

        return await rootCommand.InvokeAsync(args).ConfigureAwait(false);
    }

    private static async Task RunScanCommandAsync(string path)
    {
        if (!Directory.Exists(path))
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] The path '{path}' does not exist.");
            return;
        }

        AnsiConsole.MarkupLine($"[bold blue]CleanSlate Scanner[/] - Target: [yellow]{path}[/]");
        AnsiConsole.WriteLine();

        // Manual DI setup for MVP simplicity
        StorageScanner scanner = new();
        IStorageClassificationRule[] rules = [new PathRule(), new ExtensionRule()];
        CategorizationEngine engine = new(rules);

        StorageScanOptions options = new()
        {
            TargetPath = path,
            IncludeHiddenFiles = true,
            IncludeSystemFiles = true,
            IncludeProtectedPaths = false,
            MinimumFileSize = 0,
            CalculateHashes = false
        };

        StorageScanResult? scanResult = null;

        await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                ProgressTask scanTask = ctx.AddTask("[green]Scanning filesystem...[/]", new ProgressTaskSettings { MaxValue = 100 });
                scanTask.IsIndeterminate = true; // We don't know the total size upfront

                // In a real app, the scanner might stream files to the engine.
                // For the MVP, we just do a basic scan count to show progress, then we'll categorize the results.
                // However, StorageScanner only returns counts currently. We need to categorize on the fly or adjust the scanner.
                // Since StorageScanner only returns counts right now (StorageScanResult), we will just simulate the engine.

                scanResult = await scanner.ScanAsync(options, null, default).ConfigureAwait(false);

                scanTask.IsIndeterminate = false;
                scanTask.Value = 100;
                scanTask.StopTask();
            }).ConfigureAwait(false);

        if (scanResult != null)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[bold green]Scan Complete![/]");
            AnsiConsole.MarkupLine($"Files Scanned: [yellow]{scanResult.TotalFiles:N0}[/]");
            AnsiConsole.MarkupLine($"Directories: [yellow]{scanResult.TotalDirectories:N0}[/]");

            // Format bytes to a readable format
            double sizeMb = scanResult.TotalBytes / 1024.0 / 1024.0;
            AnsiConsole.MarkupLine($"Total Size: [yellow]{sizeMb:N2} MB[/]");
            AnsiConsole.MarkupLine($"Duration: [yellow]{scanResult.ScanDuration.TotalSeconds:N2}s[/]");
        }
    }
}
