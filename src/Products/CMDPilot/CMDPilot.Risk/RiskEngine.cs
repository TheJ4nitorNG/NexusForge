using Company.CMDPilot.Commands;

namespace Company.CMDPilot.Risk;

/// <summary>
/// Default implementation of the risk engine.
/// </summary>
public sealed class RiskEngine : IRiskEngine
{
    private static readonly string[] SafeCommands =
    [
        "Get-Process",
        "Get-Service",
        "Get-Content",
        "Get-ChildItem",
        "Get-Item",
        "Where-Object",
        "Sort-Object",
        "Select-Object",
        "Format-Table",
        "Format-List",
        "Write-Host",
        "Write-Output"
    ];

    /// <inheritdoc />
    public RiskResult Evaluate(CommandProposal proposal, bool isObfuscated)
    {
        if (isObfuscated)
        {
            return new RiskResult(RiskLevel.Critical, "Command exhibits obfuscation or dynamic invocation patterns.");
        }

        RiskLevel calculatedLevel = RiskLevel.Safe;
        string justification = "Command is considered safe.";

        if (proposal.RequiredPrivilege == PrivilegeLevel.System)
        {
            calculatedLevel = RiskLevel.Critical;
            justification = "Command requires SYSTEM privileges.";
        }
        else if (proposal.RequiredPrivilege == PrivilegeLevel.Administrator)
        {
            calculatedLevel = RiskLevel.Moderate;
            justification = "Command requires Administrator privileges.";
        }

        if (proposal.Effects.Any(e => e.Risk == RiskLevel.Critical))
        {
            return new RiskResult(RiskLevel.Critical, "Command has critical destructive effects.");
        }
        if (proposal.Effects.Any(e => e.Risk == RiskLevel.High))
        {
            return new RiskResult(RiskLevel.High, "Command has high risk effects.");
        }
        if (proposal.Effects.Any(e => e.Risk == RiskLevel.Moderate))
        {
            calculatedLevel = (RiskLevel)Math.Max((int)calculatedLevel, (int)RiskLevel.Moderate);
            justification = "Command has modifying effects.";
        }

        if (calculatedLevel == RiskLevel.Safe)
        {
            bool isKnownSafe = SafeCommands.Any(c => proposal.CommandText.Contains(c, StringComparison.OrdinalIgnoreCase));
            if (!isKnownSafe)
            {
                return new RiskResult(RiskLevel.High, "Command contains unknown or unverified instructions.");
            }
        }

        return new RiskResult(calculatedLevel, justification);
    }
}
