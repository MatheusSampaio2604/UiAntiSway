namespace Application.ViewModels
{
    public class vmWritePlc
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? Value { get; set; }
    }

    public class vmApiRequestWritePlc
    {
        public string? AddressPlc { get; set; }
        public string? Type { get; set; }
        public string? Value { get; set; }
    }
}
