namespace Jaunts.Core.Authorization;

[Flags]
public enum Permissions
{
    None = 0,
    ViewRoles = 1,
    ManageRoles = 2,
    ViewUsers = 4,
    ManageUsers = 8,
    ConfigureAccessControl = 16,
    InitiateRefund = 32,
    SplitPayments = 64,
    ViewAccessControl = 128,
    All = ~None
}