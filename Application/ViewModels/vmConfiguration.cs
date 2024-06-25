using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
