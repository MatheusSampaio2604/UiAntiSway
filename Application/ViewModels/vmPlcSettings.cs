using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class vmPlcSettings
    {
        [DisplayName("IP Connection")]
        [Required(ErrorMessage = "Required!")]
        public required string Ip1 { get; set; }

        [DisplayName("CPU type")]
        [Required(ErrorMessage = "Required!")]
        public required string CpuType { get; set; }

        [DisplayName("Rack number")]
        [Required(ErrorMessage = "Required!")]
        public required short Rack { get; set; }

        [DisplayName("Slot number")]
        [Required(ErrorMessage = "Required!")]
        public required short Slot { get; set; }

        [DisplayName("Driver")]
        [Required(ErrorMessage = "Required!")]
        public required string Driver { get; set; }
    }
}
