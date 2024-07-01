using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface InterConfigureOptionsServices
    {
        Task<List<vmDrivers>> GetDriversOptions();
        Task<List<vmCpuTypes>> GetCpuTypesOptions();
    }
}
