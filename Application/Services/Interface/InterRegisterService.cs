using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface InterRegisterService
    {
        Task<bool> RegisterAsync(vmRegisterUser registerUser);
    }
}
