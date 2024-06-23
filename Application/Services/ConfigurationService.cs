using Application.Services.Interface;
using Infra.RequestApi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ConfigurationService : InterConfigurationService
    {
        private readonly InterGeneralApi _GeneralApi;

        public ConfigurationService(InterGeneralApi generalApi)
        {
            _GeneralApi = generalApi;
        }


    }
}
