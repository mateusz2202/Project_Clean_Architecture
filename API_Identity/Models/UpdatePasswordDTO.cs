﻿namespace API_Identity.Models;

public class UpdatePasswordDTO
{
    public string? OldPassword { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
}
