namespace Jaunts.Core.Authorization;

public class AuthorizesAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
{
    public AuthorizesAttribute() { }

    public AuthorizesAttribute(string policy) : base(policy) { }

    public AuthorizesAttribute(Permissions permission)
    {
        Permissions = permission;
    }

    public Permissions Permissions
    {
        get
        {
            return !string.IsNullOrEmpty(Policy) 
                ? PolicyNameHelper.GetPermissionsFrom(Policy) 
                : Permissions.None;
        }
        set
        {
            Policy = value != Permissions.None 
                ? PolicyNameHelper.GeneratePolicyNameFor(value)
                : string.Empty;
        }
    }
}
