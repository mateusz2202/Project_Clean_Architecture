namespace Identity.Application.Requests;

public class ToggleUserStatusRequest
{
    public bool ActivateUser { get; set; }
    public string UserId { get; set; } = string.Empty;
}

