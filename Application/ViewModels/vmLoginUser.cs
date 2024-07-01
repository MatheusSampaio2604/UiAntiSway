namespace Application.ViewModels
{
    public class vmLoginUser
    {
#if DEBUG
        public string UserName { get; set; } = "Admin";
        public string Password { get; set; } = "Admin";
#elif RELEASE
        public string? UserName { get; set; } = String.Empty;
        public string? Password { get; set; } = String.Empty;
#endif
    }

    public class LoginResult
    {
        public bool Succeeded { get; set; }
        public string? Name { get; set; }
        public string? Token { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
