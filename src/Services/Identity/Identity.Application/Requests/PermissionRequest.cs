namespace Identity.Application.Requests;

public class PermissionRequest
{
    public string RoleId { get; set; }=string.Empty;
    public IList<RoleClaimRequest> RoleClaims { get; set; }
}