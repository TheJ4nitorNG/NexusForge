using Company.CMDPilot.Commands;

namespace Company.CMDPilot.Risk;

/// <summary>
/// Defines the contract for evaluating the risk of a command proposal.
/// </summary>
public interface IRiskEngine
{
    /// <summary>
    /// Evaluates the risk of a given command proposal.
    /// </summary>
    /// <param name="proposal">The command proposal.</param>
    /// <param name="isObfuscated">Whether the command was flagged for obfuscation during parsing.</param>
    /// <returns>The calculated risk result.</returns>
    RiskResult Evaluate(CommandProposal proposal, bool isObfuscated);
}
