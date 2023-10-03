using API_Identity.Models;

namespace API_Identity.Interfaces;

public interface IUserService
{  
    public Task<string> GetToken(LoginDTO dto);
    public Task RegisterUser(RegisterUserDTO dto);
    public Task ChangePassword(string email, UpdatePasswordDTO dto);

}
