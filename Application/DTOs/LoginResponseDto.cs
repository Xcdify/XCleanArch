using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMP.Application.DTOs
{
    public class LoginResponseDto
    {
        public class LoginResponse
        {
            public string Token { get; set; }
            public string Username { get; set; }
            public long UserId { get; set; }
            public string Role { get; set; }
        }
    }
}
