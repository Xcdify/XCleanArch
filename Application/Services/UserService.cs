using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using XMP.Application.DTOs;
using XMP.Application.Interfaces;
using XMP.Domain.Entities;
using XMP.Domain.Repositories;
using BCrypt.Net;

namespace XMP.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<UserResponseDto>> GetAllUsersAsync(int pageNumber, int pageSize, string? search)
        {
            var users = await _userRepository.GetAllAsync(pageNumber, pageSize, search);
            var activeUsers = users.Items.Where(u => u.IsActive ?? false).ToList();
            return new PaginationResponseDto<UserResponseDto>
            {
                Items = _mapper.Map<IEnumerable<UserResponseDto>>(users.Items),
                TotalCount = users.TotalCount,
                End = users.PageSize,
                Start = users.PageNumber
            };
           // return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserResponseDto> GetUserByIdAsync(long id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> AddUserAsync(CreateUserDto createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            
            var id = await _userRepository.AddAsync(user);
            user.Id = id;
            
           /* if (createUserDto.CompanyIds != null && createUserDto.CompanyIds.Any())
            {
                await _userRepository.AddUserCompaniesAsync(id, createUserDto.CompanyIds);
            }
            if (createUserDto.ProjectIds != null && createUserDto.ProjectIds.Any())
            {
                await _userRepository.AddUserProjectsAsync(id, createUserDto.ProjectIds);
            }

            if (createUserDto.RoleIds != null && createUserDto.RoleIds.Any())
            {
                await _userRepository.AddUserRolesAsync(id, createUserDto.RoleIds);
            }*/
            
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            var userToUpdate = _mapper.Map<User>(updateUserDto);
            
            if (!string.IsNullOrEmpty(updateUserDto.PasswordHash))
            {
                userToUpdate.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDto.PasswordHash);
            }
            
            userToUpdate.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(userToUpdate);
            
           /* if (updateUserDto.CompanyIds != null)
            {
                await _userRepository.UpdateUserCompaniesAsync(updateUserDto.Id, updateUserDto.CompanyIds);
            }

            if (updateUserDto.ProjectIds != null)
            {
                await _userRepository.UpdateUserProjectsAsync(updateUserDto.Id, updateUserDto.ProjectIds);
            }

            if (updateUserDto.RoleIds != null)
            {
                await _userRepository.UpdateUserRolesAsync(updateUserDto.Id, updateUserDto.RoleIds);
            }*/
        }

        public async Task DeleteUserAsync(long id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.IsActive = false; // Mark user as inactive
            await _userRepository.UpdateAsync(user); // Update user in the repository

            // Optionally, mark related entities as inactive as well
            await _userRepository.UpdateUserCompaniesAsync(id, null); // Assuming you want to clear associations
            await _userRepository.UpdateUserProjectsAsync(id, null);
            await _userRepository.UpdateUserRolesAsync(id, null);
        }

        //update password for userid
        public async Task UpdateUserPasswordAsync(long id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var firstName = user.FirstName; // Assuming FirstName is a property of User
            var newPassword = $"{firstName}@321";
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
        }

     /*   public async Task UpdateAllUserPasswordsAsync()
        {
            var usersResponse = await _userRepository.GetAllAsync(1, int.MaxValue); // Fetch all users with pageNumber and pageSize
            foreach (var user in usersResponse.Items) // Accessing the Items property
            {
                string newPassword = user.FirstName + "@321"; // Construct new password
                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword); // Update password logic
                await _userRepository.UpdateAsync(user); // Save changes
            }
        }*/
    }
}
