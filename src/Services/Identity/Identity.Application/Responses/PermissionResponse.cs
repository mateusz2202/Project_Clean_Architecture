using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Responses;

public class PermissionResponse
{
    public string RoleId { get; set; }=string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public List<RoleClaimResponse> RoleClaims { get; set; }
}
