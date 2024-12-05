using System.Collections.Generic;
using System.Threading.Tasks;
using XMP.Domain.Entities;

namespace XMP.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<PaginationResponse<User>> GetAllAsync(int pageNumber, int pageSize, string? search);
        Task<User> GetByIdAsync(long id);
        Task<int> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(long id);
        Task<User> GetByUsernameAsync(string username);
        Task<bool> CreateUserAsync(User user);
        Task AddUserCompaniesAsync(long userId, List<long> companyIds);
        Task UpdateUserCompaniesAsync(long userId, List<long> companyIds);
        Task DeleteUserCompaniesAsync(long userId);
        Task AddUserProjectsAsync(long userId, List<long> projectIds);
        Task UpdateUserProjectsAsync(long userId, List<long> projectIds);
        Task DeleteUserProjectsAsync(long userId);
        Task AddUserRolesAsync(long userId, List<long> roleIds);
        Task UpdateUserRolesAsync(long userId, List<long> roleIds);
        Task DeleteUserRolesAsync(long userId);
    }
}

