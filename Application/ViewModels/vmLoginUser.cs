namespace Application.ViewModels
{
    public class vmLoginUser
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }

    public class LoginResult
    {
        public bool Succeeded { get; set; }
        public string? Name { get; set; }
        public string? Token { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
