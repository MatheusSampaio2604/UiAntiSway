using Application.ViewModels;

namespace Application.Services.Interface
{
    public interface InterLoginService
    {
        Task<LoginResult> LoginAsync(vmLoginUser loginUser);
    }
}
