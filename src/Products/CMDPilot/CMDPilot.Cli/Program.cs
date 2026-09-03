using System.CommandLine;
using Company.CMDPilot.Commands;
using Company.CMDPilot.PowerShell;
using Company.CMDPilot.Risk;
using Spectre.Console;

namespace Company.CMDPilot.Cli;

/// <summary>
/// The main entry point for the CMDPilot CLI application.
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
        RootCommand rootCommand = new("CMDPilot - AI-assisted PowerShell and command-line operations.");

        Command analyzeCommand = new("analyze", "Analyzes a command script for risk.");
        Argument<string> scriptArgument = new("script", "The command or script to analyze.");
        analyzeCommand.AddArgument(scriptArgument);

        analyzeCommand.SetHandler(RunAnalyzeCommand, scriptArgument);

        rootCommand.AddCommand(analyzeCommand);

        return await rootCommand.InvokeAsync(args).ConfigureAwait(false);
    }

    private static void RunAnalyzeCommand(string script)
    {
        AnsiConsole.MarkupLine("[bold blue]CMDPilot Analysis[/]");
        AnsiConsole.WriteLine();

        RiskEngine riskEngine = new();

        bool isObfuscated = PowerShellAstAnalyzer.DetectObfuscation(script);
        IReadOnlyList<string> extractedCommands = PowerShellAstAnalyzer.ExtractCommands(script);

        CommandProposal proposal = new()
        {
            CommandText = script,
            Purpose = "User-provided script for analysis.",
            RequiredPrivilege = PrivilegeLevel.Standard,
            Effects = []
        };

        RiskResult riskResult = riskEngine.Evaluate(proposal, isObfuscated);

        string color = riskResult.Level switch
        {
            RiskLevel.Safe => "green",
            RiskLevel.Low => "blue",
            RiskLevel.Moderate => "yellow",
            RiskLevel.High => "orange3",
            RiskLevel.Critical => "red",
            RiskLevel.Unknown => throw new NotImplementedException(),
            _ => "grey"
        };

        Grid grid = new();
        grid.AddColumn();
        grid.AddColumn();

        grid.AddRow("[bold]Input Script:[/]", $"[dim]{script}[/]");
        grid.AddRow("[bold]Obfuscation Detected:[/]", isObfuscated ? "[red]Yes[/]" : "[green]No[/]");
        grid.AddRow("[bold]Extracted Commands:[/]", string.Join(", ", extractedCommands));
        grid.AddRow("[bold]Justification:[/]", riskResult.Justification);

        Panel panel = new(grid)
        {
            Header = new PanelHeader($"[bold {color}]Risk Level: {riskResult.Level.ToString().ToUpperInvariant()}[/]"),
            Border = BoxBorder.Rounded,
            Padding = new Padding(1, 1, 1, 1)
        };

        AnsiConsole.Write(panel);

        if (riskResult.Level is RiskLevel.High or RiskLevel.Critical)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[bold red]WARNING:[/] This command is classified as {riskResult.Level}. Execution without explicit review is strongly discouraged.");
        }
    }
}
