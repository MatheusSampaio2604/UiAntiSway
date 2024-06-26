using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
