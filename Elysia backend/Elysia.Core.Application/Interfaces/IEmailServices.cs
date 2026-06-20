using Elysia.Core.Application.Dtos.Email;

namespace Elysia.Core.Application.Interfaces
{
    public interface IEmailServices
    {
        Task<bool> SendAsync(EmailDto? dto);
    }
}