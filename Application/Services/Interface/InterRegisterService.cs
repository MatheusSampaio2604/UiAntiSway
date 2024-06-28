using Application.ViewModels;

namespace Application.Services.Interface
{
    public interface InterRegisterService
    {
        Task<bool> RegisterAsync(vmRegisterUser registerUser);
    }
}
