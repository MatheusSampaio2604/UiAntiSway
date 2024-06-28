namespace Application.ViewModels
{
    public class vmConfiguration
    {
        public string Address { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;

        public List<vmConfiguration> vmConfigurations { get; set; }
    }
}
