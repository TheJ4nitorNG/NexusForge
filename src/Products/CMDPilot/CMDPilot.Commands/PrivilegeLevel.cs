namespace Company.CMDPilot.Commands;

/// <summary>
/// Represents the privilege level required to execute a command.
/// </summary>
public enum PrivilegeLevel
{
    /// <summary>The required privilege level is unknown.</summary>
    Unknown = 0,
    /// <summary>Standard user privileges.</summary>
    Standard = 1,
    /// <summary>Administrator privileges required.</summary>
    Administrator = 2,
    /// <summary>SYSTEM level privileges required.</summary>
    System = 3
}
