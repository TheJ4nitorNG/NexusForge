namespace Company.CMDPilot.Commands;

/// <summary>
/// Represents a proposed command to be evaluated and potentially executed.
/// </summary>
public sealed record CommandProposal
{
    /// <summary>Gets the raw command text.</summary>
    public required string CommandText { get; init; }

    /// <summary>Gets the purpose of this command.</summary>
    public required string Purpose { get; init; }

    /// <summary>Gets the required privilege level.</summary>
    public PrivilegeLevel RequiredPrivilege { get; init; } = PrivilegeLevel.Standard;

    /// <summary>Gets the expected effects of this command.</summary>
    public IReadOnlyList<CommandEffect> Effects { get; init; } = [];
}
