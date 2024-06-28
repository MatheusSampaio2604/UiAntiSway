using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class vmPlc
    {
        public int Id { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "*Necessary!")]
        public required string Name { get; set; }

        [DisplayName("Address Plc")]
        [Required(ErrorMessage = "*Necessary!")]
        public required string AddressPlc { get; set; }

        [Required(ErrorMessage = "*Necessary!")]
        public required string Type { get; set; }

        [DisplayName("Last value")]
        public string? Value { get; set; }
    }
}
