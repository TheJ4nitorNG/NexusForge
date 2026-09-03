namespace Company.CMDPilot.Commands;

/// <summary>
/// Represents the assessed risk level of a command or effect.
/// </summary>
public enum RiskLevel
{
    /// <summary>The risk level is unknown.</summary>
    Unknown = 0,
    /// <summary>The command is safe (e.g., read-only).</summary>
    Safe = 1,
    /// <summary>The command has low risk.</summary>
    Low = 2,
    /// <summary>The command has moderate risk (e.g., configuration changes).</summary>
    Moderate = 3,
    /// <summary>The command has high risk.</summary>
    High = 4,
    /// <summary>The command is highly destructive or dangerous.</summary>
    Critical = 5
}
