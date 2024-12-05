using System.Collections.Generic;
using System.Threading.Tasks;
using XMP.Application.DTOs;

namespace XMP.Application.Interfaces
{
    public interface IUserService
    {
        Task<PaginationResponseDto<UserResponseDto>> GetAllUsersAsync(int pageNumber, int pageSize, string? search);
        Task<UserResponseDto> GetUserByIdAsync(long id);
        Task<UserResponseDto> AddUserAsync(CreateUserDto userDto);
        Task UpdateUserAsync(UpdateUserDto userDto);
        Task DeleteUserAsync(long id);
        Task UpdateUserPasswordAsync(long id);
        //Task UpdateAllUserPasswordsAsync();
    }
}
