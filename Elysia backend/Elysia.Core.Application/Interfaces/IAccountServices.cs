using Elysia.Core.Application.Dtos.User;

namespace Elysia.Core.Application.Interfaces
{
    public interface IAccountServices
    {
        Task<UserResponseDto> ActivarUser(string usuarioId);
        Task<LoginResponseDto> Authenticate(LoginDto dto);
        Task<ConfirmResponseDto?> confirmAccounAsync(ConfirmRequestDto? dto);
        Task<UserResponseDto> DeleteAsync(string id);
        Task<EditResponseDto?> EditUser(SaveUserRequestDto? saveUser, bool? IsCreated = false);
        Task<UserResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto request);
        Task<List<UserDto>> GetAllUser(bool? IsActive = true);
        Task<UserDto?> GetUserByEmail(string gmail);
        Task<UserDto?> GetUserById(string id);
        Task<UserDto?> GetUserByUserName(string userName);
        Task<UserResponseDto> InhativarUser(string usuarioId);
        Task<RegisterResponseDto?> RegisterUser(SaveUserRequestDto? saveUser);
        Task<UserResponseDto?> RessetPassowrd(RessetPasswordRequestDto? request);
    }
}