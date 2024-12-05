using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using XMP.Domain.Entities;
using XMP.Domain.Repositories;
using XMP.Infrastructure.DbContext;

namespace XMP.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperDbContext _dbContext;

        public UserRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<PaginationResponse<User>> GetAllAsync(int pageNumber, int pageSize, string? search)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("PageNumber and PageSize must be positive integers.");
            }
            var skip = (pageNumber - 1) * pageSize;
            var whereConditions = new List<string>();
            var parameters = new DynamicParameters();
            parameters.Add("Skip", skip);
            parameters.Add("Take", pageSize);

            if (!string.IsNullOrWhiteSpace(search))
            {
                whereConditions.Add("(name ILIKE CONCAT('%', @Search, '%') OR @Search IS NULL) OR (first_name ILIKE CONCAT('%', @Search, '%') OR @Search IS NULL) OR (last_name ILIKE CONCAT('%', @Search, '%') OR @Search IS NULL)");
                parameters.Add("Search", search);
            }

            using var connection = _dbContext.GetConnection();
            // Get the total count of users
            var totalCountQuery = "SELECT COUNT(*) FROM users";
            var totalCount = await connection.ExecuteScalarAsync<int>(totalCountQuery);
            var whereClause = whereConditions.Count > 0
                ? $"WHERE {string.Join(" AND ", whereConditions)}"
                : string.Empty;

            var query = $@"SELECT 
        id, 
        email,
        encrypted_password AS Password,
        reset_password_token AS ResetPasswordToken,
        reset_password_sent_at AS ResetPasswordSentAt,
        remember_created_at AS RememberCreatedAt,
        created_at AS CreatedAt,
        updated_at AS UpdatedAt,
        avatar,
        username,
        name,
        first_name AS FirstName,
        last_name AS LastName,
        gender,
        mobile_number AS MobileNumber,
        role,
        is_active AS IsActive
    FROM users 
    {whereClause}
    ORDER BY created_at DESC 
    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";
            var users = await connection.QueryAsync<User>(query, parameters);

           /* foreach (var user in users)
            {
                var companyQuery = @"SELECT company_id FROM users_companies WHERE user_id = @UserId";
                var companyIds = await connection.QueryAsync<long>(companyQuery, new { UserId = user.Id });
                user.CompanyIds = companyIds.ToList();

                var projectQuery = @"SELECT project_id FROM users_projects WHERE user_id = @UserId";
                var projectIds = await connection.QueryAsync<long>(projectQuery, new { UserId = user.Id });
                user.ProjectIds = projectIds.ToList();

                var roleQuery = @"SELECT role_id FROM users_roles WHERE user_id = @UserId";
                var roleIds = await connection.QueryAsync<long>(roleQuery, new { UserId = user.Id });
                user.RoleIds = roleIds.ToList();
            }*/


            return new PaginationResponse<User>(users, totalCount, pageNumber, pageSize);
        }

        public async Task<User> GetByIdAsync(long id)
        {
            using var connection = _dbContext.GetConnection();
            var userQuery = @"SELECT 
        id, 
        email,
        encrypted_password AS Password,
        reset_password_token AS ResetPasswordToken,
        reset_password_sent_at AS ResetPasswordSentAt,
        remember_created_at AS RememberCreatedAt,
        created_at AS CreatedAt,
        updated_at AS UpdatedAt,
        avatar,
        username,
        name,
        first_name AS FirstName,
        last_name AS LastName,
        gender,
        mobile_number AS MobileNumber,
        role,
        is_active AS IsActive
    FROM users 
    WHERE id = @Id";
            var user = await connection.QuerySingleOrDefaultAsync<User>(userQuery, new { Id = id });
/*
            if (user != null)
            {
                var companyQuery = @"SELECT company_id FROM users_companies WHERE user_id = @UserId";
                var companyIds = await connection.QueryAsync<long>(companyQuery, new { UserId = id });
                user.CompanyIds = companyIds.ToList();

                var projectQuery = @"SELECT project_id FROM users_projects WHERE user_id = @UserId";
                var projectIds = await connection.QueryAsync<long>(projectQuery, new { UserId = id });
                user.ProjectIds = projectIds.ToList();

                var roleQuery = @"SELECT role_id FROM users_roles WHERE user_id = @UserId";
                var roleIds = await connection.QueryAsync<long>(roleQuery, new { UserId = id });
                user.RoleIds = roleIds.ToList();
            }*/

            return user;
        }

        public async Task<int> AddAsync(User user)
        {
            using var connection = _dbContext.GetConnection();
            var query = @"INSERT INTO users 
                        (email, encrypted_password, reset_password_token, reset_password_sent_at, 
                         remember_created_at, created_at, updated_at, avatar, username, 
                         name, first_name, last_name, gender, mobile_number, role, is_active) 
                         VALUES 
                        (@Email, @EncryptedPassword, @ResetPasswordToken, @ResetPasswordSentAt, 
                         @RememberCreatedAt, @CreatedAt, @UpdatedAt, @Avatar, @Username, 
                         @Name, @FirstName, @LastName, @Gender, @MobileNumber, @Role, @IsActive)
                         RETURNING id;";
            
            var id = await connection.ExecuteScalarAsync<int>(query, user);
            user.Id = id; // Update the user object with the new ID
            return id;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            const string sql = @"
    INSERT INTO users (username, encrypted_password, email, created_at, updated_at)
    VALUES (@Username, @EncryptedPassword, @Email, @CreatedAt, @UpdatedAt)";

            using var connection = _dbContext.GetConnection();

            var parameters = new
            {
                Username = user.Name,
                EncryptedPassword = user.Password, // Ensure the parameter name matches
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            try
            {
                var result = await connection.ExecuteAsync(sql, parameters);
                return result > 0; // Return true if rows were affected
            }
            catch (Exception ex)
            {
                // Handle or log the exception as necessary
                throw new Exception("An error occurred while creating the user.", ex);
            }
        }


        public async Task UpdateAsync(User user)
        {
            try
            {
                using var connection = _dbContext.GetConnection();

                // Create a dictionary to store the fields to update
                var updates = new Dictionary<string, object>();

                // Add only the non-null properties
                if (!string.IsNullOrEmpty(user.Email))
                    updates.Add("email", user.Email);
                
                if (!string.IsNullOrEmpty(user.Password))
                    updates.Add("encrypted_password", user.Password);
                    
               /* if (!string.IsNullOrEmpty(user.Username))
                    updates.Add("username", user.Username);*/
                    
                if (!string.IsNullOrEmpty(user.Name))
                    updates.Add("name", user.Name);
                    
                if (!string.IsNullOrEmpty(user.FirstName))
                    updates.Add("first_name", user.FirstName);
                    
                if (!string.IsNullOrEmpty(user.LastName))
                    updates.Add("last_name", user.LastName);
                    
                if (!string.IsNullOrEmpty(user.Gender))
                    updates.Add("gender", user.Gender);
                    
                if (!string.IsNullOrEmpty(user.MobileNumber))
                    updates.Add("mobile_number", user.MobileNumber);
                    
                if (!string.IsNullOrEmpty(user.Role))
                    updates.Add("role", user.Role);

                if (user.IsActive.HasValue)
                    updates.Add("is_active", user.IsActive.Value);

                // If no updates, return
                if (!updates.Any())
                    return;

                // Add the updated_at timestamp
                updates.Add("updated_at", DateTime.UtcNow);

                // Build the update query dynamically
                var setClause = string.Join(", ", updates.Keys.Select(k => $"{k} = @{k}"));
                var query = $"UPDATE users SET {setClause} WHERE id = @Id";

                // Create parameters object with all values
                var parameters = new DynamicParameters(updates);
                parameters.Add("Id", user.Id);

                await connection.ExecuteAsync(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user.", ex);
            }
        }

        public async Task DeleteAsync(long id)
        {
            using var connection = _dbContext.GetConnection();
            //var query = "DELETE FROM users WHERE id = @Id";
            var query = "UPDATE users SET is_active = false WHERE id = @Id"; 
            await connection.ExecuteAsync(query, new { Id = id });
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            const string sql = "SELECT id, username, encrypted_password AS Password, email FROM users WHERE username = @Username";
            using var connection = _dbContext.GetConnection();
            var result = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });

          /*  if (result != null)
            {
                var companyQuery = @"SELECT company_id FROM users_companies WHERE user_id = @UserId";
                var companyIds = await connection.QueryAsync<long>(companyQuery, new { UserId = result.Id });
                result.CompanyIds = companyIds.ToList();

                var projectQuery = @"SELECT project_id FROM users_projects WHERE user_id = @UserId";
                var projectIds = await connection.QueryAsync<long>(projectQuery, new { UserId = result.Id });
                result.ProjectIds = projectIds.ToList();

                var roleQuery = @"SELECT role_id FROM users_roles WHERE user_id = @UserId";
                var roleIds = await connection.QueryAsync<long>(roleQuery, new { UserId = result.Id });
                result.RoleIds = roleIds.ToList();
            }*/

            return result;
        }

        public async Task AddUserCompaniesAsync(long userId, List<long> companyIds)
        {
            using var connection = _dbContext.GetConnection();
            var query = @"INSERT INTO users_companies (user_id, company_id, created_at, updated_at) 
                         VALUES (@UserId, @CompanyId, @CreatedAt, @UpdatedAt)";
                         
            foreach (var companyId in companyIds)
            {
                await connection.ExecuteAsync(query, new { 
                    UserId = userId, 
                    CompanyId = companyId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }
        public async Task AddUserProjectsAsync(long userId, List<long> projectIds)
        {
            using var connection = _dbContext.GetConnection();
            var query = @"INSERT INTO users_projects (user_id, project_id, created_at, updated_at) 
                         VALUES (@UserId, @ProjectId, @CreatedAt, @UpdatedAt)";
                         
            foreach (var projectId in projectIds)
            {
                await connection.ExecuteAsync(query, new { 
                    UserId = userId, 
                    ProjectId = projectId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }

        public async Task UpdateUserProjectsAsync(long userId, List<long> projectIds)
        {
            using var connection = _dbContext.GetConnection();
            
            // Delete existing associations
            await DeleteUserProjectsAsync(userId);
            
            // Add new associations
            if (projectIds != null && projectIds.Any())
            {
                await AddUserProjectsAsync(userId, projectIds);
            }
        }

        public async Task DeleteUserProjectsAsync(long userId)
        {
            using var connection = _dbContext.GetConnection();
            var query = "DELETE FROM users_projects WHERE user_id = @UserId";
            await connection.ExecuteAsync(query, new { UserId = userId });
        }

        public async Task AddUserRolesAsync(long userId, List<long> roleIds)
        {
            using var connection = _dbContext.GetConnection();
            var query = @"INSERT INTO users_roles (user_id, role_id, updated_at) 
                         VALUES (@UserId, @RoleId, @UpdatedAt)";
                         
            foreach (var roleId in roleIds)
            {
                await connection.ExecuteAsync(query, new { 
                    UserId = userId, 
                    RoleId = roleId,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }

        public async Task UpdateUserRolesAsync(long userId, List<long> roleIds)
        {
            using var connection = _dbContext.GetConnection();
            
            // Delete existing associations
            await DeleteUserRolesAsync(userId);
            
            // Add new associations
            if (roleIds != null && roleIds.Any())
            {
                await AddUserRolesAsync(userId, roleIds);
            }
        }

        public async Task DeleteUserRolesAsync(long userId)
        {
            using var connection = _dbContext.GetConnection();
            var query = "DELETE FROM users_roles WHERE user_id = @UserId";
            await connection.ExecuteAsync(query, new { UserId = userId });
        }

        public async Task UpdateUserCompaniesAsync(long userId, List<long> companyIds)
        {
            using var connection = _dbContext.GetConnection();
            
            // Delete existing associations
            await DeleteUserCompaniesAsync(userId);
            
            // Add new associations
            if (companyIds != null && companyIds.Any())
            {
                await AddUserCompaniesAsync(userId, companyIds);
            }
        }

        public async Task DeleteUserCompaniesAsync(long userId)
        {
            using var connection = _dbContext.GetConnection();
            var query = "DELETE FROM users_companies WHERE user_id = @UserId";
            await connection.ExecuteAsync(query, new { UserId = userId });
        }
    }
}
