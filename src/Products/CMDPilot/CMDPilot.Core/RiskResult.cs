namespace Company.CMDPilot.Core;

/// <summary>
/// Represents the result of a risk analysis on a proposed command.
/// </summary>
/// <param name="Level">The determined risk level.</param>
/// <param name="Justification">The explanation for the assigned risk level.</param>
public sealed record RiskResult(
    RiskLevel Level,
    string Justification);
