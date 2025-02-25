using Microsoft.AspNetCore.Mvc;
using FoodOrderingSystem.DTOs;
using FoodOrderingSystem.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FoodOrderingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO registerUserDTO)
        {
            try
            {
                var user = await _userService.RegisterUser(registerUserDTO);
                return Ok(new { message = "User registered successfully!", user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
      [HttpPost("login")]
public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
{
    try
    {
        var (token, role, email) = await _userService.LoginUser(loginUserDTO);
        return Ok(new { message = "User logged in successfully!", token, role, email });
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}
        [Authorize(Roles = "Admin")] // Only users with the "Admin" role can access this endpoint
[HttpGet("all")]
public async Task<IActionResult> GetAllUsers()
{
    try
    {
        var users = await _userService.GetAllUsers();
        return Ok(new{ users = users,
          message = "All users retrieved successfully!" }
          );
    }
    catch (Exception ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}
[Authorize(Roles = "Admin")] // Only users with the "Admin" role can access this endpoint
[HttpGet("{id}")]
public async Task<IActionResult> GetUserById(int id)
{
    try
    {
        var user = await _userService.GetUserById(id);
        return Ok(user);
    }
    catch (Exception ex)
    {
        return NotFound(new { message = ex.Message });
    }
}
[Authorize(Roles = "Admin")] // Only users with the "Admin" role can access this endpoint
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteUser(int id)
{
    try
    {
        await _userService.DeleteUser(id);
        return Ok(new { message = "User deleted successfully!" });
    }
    catch (Exception ex)
    {
        return NotFound(new { message = ex.Message });
    }
}
    }
}