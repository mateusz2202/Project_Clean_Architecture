﻿namespace BlazorApp.Client.Infrastructure.Routes;

public static class UserEndpoints
{
    public static string GetAll = "api/identity/user";

    public static string Get(string userId)
    {
        return $"api/identity/user/{userId}";
    }

    public static string GetUserRoles(string userId)
    {
        return $"api/identity/user/roles/{userId}";
    }

    public static string ExportFiltered(string searchString)
    {
        return $"{Export}?searchString={searchString}";
    }
    public static string GetProfilePicture(string userId)
    {
        return $"api/identity/user/profile-picture/{userId}";
    }

    public static string UpdateProfilePicture(string userId)
    {
        return $"api/identity/user/profile-picture/{userId}";
    }

    public static string Export(string searchString) => string.IsNullOrWhiteSpace(searchString)
                                                            ? ExportEndpoint
                                                            : ExportFiltered(searchString);

    private static string ExportEndpoint = "api/identity/user/export";
    public static string Register = "api/identity/user";
    public static string ToggleUserStatus = "api/identity/user/toggle-status";
    public static string ForgotPassword = "api/identity/user/forgot-password";
    public static string ResetPassword = "api/identity/user/reset-password";
    public static string ChangePassword = "api/identity/user/changepassword";
    public static string UpdateProfile = "api/identity/user/updateprofile";
}