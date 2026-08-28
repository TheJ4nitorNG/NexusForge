namespace Company.Platform.Abstractions;

/// <summary>
/// Represents the execution policy and risk level of a command.
/// </summary>
public enum ExecutionPolicy
{
    /// <summary>Unknown risk level.</summary>
    Unknown,
    /// <summary>Read-only operations.</summary>
    ReadOnly,
    /// <summary>Low risk modifying operations.</summary>
    LowRisk,
    /// <summary>Modifying operations.</summary>
    Modifying,
    /// <summary>Privileged operations requiring elevation.</summary>
    Privileged,
    /// <summary>Destructive operations.</summary>
    Destructive
}
