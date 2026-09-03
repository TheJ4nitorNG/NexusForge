using System.Diagnostics;
using Company.Platform.Abstractions;
using Company.CMDPilot.Commands;
using Company.CMDPilot.Risk;

namespace Company.CMDPilot.Execution;

/// <summary>
/// A secure command execution service that validates risks before launching processes.
/// </summary>
/// <param name="riskEngine">The risk classification engine.</param>
public sealed class ElevatedRunner(IRiskEngine riskEngine) : ICommandExecutionService
{
    private readonly IRiskEngine _riskEngine = riskEngine ?? throw new ArgumentNullException(nameof(riskEngine));

    /// <inheritdoc />
    public async Task<CommandExecutionResult> ExecuteAsync(
        CommandRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        string commandLine = $"{request.Command} {string.Join(" ", request.Arguments)}".Trim();

        // 1. Evaluate risk using the CMDPilot Risk Engine
        CommandProposal proposal = new()
        {
            CommandText = commandLine,
            Purpose = "Execution request via ElevatedRunner",
            RequiredPrivilege = request.RequiresElevation ? PrivilegeLevel.Administrator : PrivilegeLevel.Standard,
            Effects = []
        };

        RiskResult riskResult = _riskEngine.Evaluate(proposal, isObfuscated: false);

        // 2. Strict privilege and safety check
        if (riskResult.Level is RiskLevel.High or RiskLevel.Critical && request.Policy != ExecutionPolicy.Destructive)
        {
            string blockMsg = $"Execution blocked by CMDPilot Safety Policy. Risk Level: {riskResult.Level}. Justification: {riskResult.Justification}. To override, execute using Destructive execution policy.";
            return new CommandExecutionResult(
                -1,
                string.Empty,
                blockMsg,
                TimeSpan.Zero);
        }

        // 3. Execute the process securely
        return await Task.Run(() => ExecuteProcessAsync(request, cancellationToken), cancellationToken).ConfigureAwait(false);
    }

    private static async Task<CommandExecutionResult> ExecuteProcessAsync(
        CommandRequest request,
        CancellationToken cancellationToken)
    {
        string fileName = request.Command;
        string arguments = string.Join(" ", request.Arguments);

        // Smart-wrapping: If the command looks like a PowerShell cmdlet, launch it via powershell.exe
        bool isPowerShell = request.Command.Contains('-') || request.Command.EndsWith(".ps1", StringComparison.OrdinalIgnoreCase);
        bool alreadyShell = request.Command.Equals("powershell", StringComparison.OrdinalIgnoreCase) ||
                            request.Command.Equals("powershell.exe", StringComparison.OrdinalIgnoreCase) ||
                            request.Command.Equals("cmd", StringComparison.OrdinalIgnoreCase) ||
                            request.Command.Equals("cmd.exe", StringComparison.OrdinalIgnoreCase);

        if (isPowerShell && !alreadyShell)
        {
            fileName = "powershell.exe";
            // Escape any double quotes in the arguments to prevent injection
            string escapedCommand = $"{request.Command} {string.Join(" ", request.Arguments)}".Replace("\"", "\\\"");
            arguments = $"-NoProfile -NonInteractive -Command \"{escapedCommand}\"";
        }

        ProcessStartInfo startInfo = new()
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            using Process process = new() { StartInfo = startInfo };
            if (!process.Start())
            {
                return new CommandExecutionResult(-1, string.Empty, "Failed to start the host process.", TimeSpan.Zero);
            }

            string stdout = await process.StandardOutput.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
            string stderr = await process.StandardError.ReadToEndAsync(cancellationToken).ConfigureAwait(false);

            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
            stopwatch.Stop();

            return new CommandExecutionResult(process.ExitCode, stdout, stderr, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return new CommandExecutionResult(-1, string.Empty, $"Process execution failed: {ex.Message}", stopwatch.Elapsed);
        }
    }
}
