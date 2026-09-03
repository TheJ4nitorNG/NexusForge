namespace Company.CMDPilot.Commands;

/// <summary>
/// Represents the result of a risk evaluation.
/// </summary>
/// <param name="Level">The calculated risk level.</param>
/// <param name="Justification">The justification for the risk level.</param>
public sealed record RiskResult(RiskLevel Level, string Justification);
