using Assignment_2_BE.DTOs;

namespace Assignment_2_BE.service
{
    public interface IAuthService
    {
        string? Authenticate(LoginRequestDTO request);
    }
}
