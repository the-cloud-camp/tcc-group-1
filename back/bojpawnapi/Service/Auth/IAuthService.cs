using bojpawnapi.DTO.Auth;

namespace bojpawnapi.Service.Auth;
public interface IAuthService
{
    Task<(int, string)> Registration(RegistrationDTO model, string role);
    Task<(int, string)> Login(LoginDTO model);
}