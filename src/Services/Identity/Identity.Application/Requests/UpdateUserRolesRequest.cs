using Identity.Application.Responses;


namespace Identity.Application.Requests;

public class UpdateUserRolesRequest
{
    public string UserId { get; set; }=string.Empty;
    public IList<UserRoleModel> UserRoles { get; set; }
}

