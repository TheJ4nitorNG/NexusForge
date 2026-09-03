namespace Company.CMDPilot.Commands;

/// <summary>
/// Represents an expected effect of a proposed command.
/// </summary>
public sealed record CommandEffect
{
    /// <summary>Gets a description of the effect.</summary>
    public required string Description { get; init; }

    /// <summary>Gets the risk level associated with this specific effect.</summary>
    public required RiskLevel Risk { get; init; }

    /// <summary>Gets a value indicating whether this effect is considered destructive.</summary>
    public bool IsDestructive { get; init; }
}
