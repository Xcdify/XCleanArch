using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMP.Domain.Entities;
using static XMP.Application.DTOs.LoginResponseDto;

namespace XMP.Application.Interfaces
{
    public interface IAuthService
    {
        //Task<string> LoginAsync(LoginRequest request);
        Task<bool> RegisterAsync(RegisterRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
 