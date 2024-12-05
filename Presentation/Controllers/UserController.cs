using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XMP.Application.DTOs;
using XMP.Application.Interfaces;

namespace XMP.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10, string? search = null)
        {
            try
            {


                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest("Invalid page number or page size.");
                }

                var users = await _userService.GetAllUsersAsync(pageNumber, pageSize, search);
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> Create([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var user = await _userService.AddUserAsync(createUserDto);
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the user.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                if (id != updateUserDto.Id)
                {
                    return BadRequest("ID mismatch");
                }

                await _userService.UpdateUserAsync(updateUserDto);
                return Ok("Successfuly completed");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok("Successfuly completed");
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update-password/{id}")]
        public async Task<IActionResult> UpdatePassword(long id)
        {
            try
            {
                await _userService.UpdateUserPasswordAsync(id);
                //await _userService.UpdateAllUserPasswordsAsync();
                return Ok("Password updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the password.");
            }
        }

       /* [HttpPut("update-all-passwords")]
        public async Task<IActionResult> UpdateAllPasswords()
        {
            try
            {
                await _userService.UpdateAllUserPasswordsAsync(); // Call to update all user passwords
                return Ok("All user passwords updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating all passwords.");
            }
        }*/
    }
}
