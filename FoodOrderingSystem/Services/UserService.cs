using System;
using System.Threading.Tasks;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Data;
using FoodOrderingSystem.DTOs;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace FoodOrderingSystem.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<User> RegisterUser(RegisterUserDTO registerUserDTO)
        {
            if (registerUserDTO == null)
            {
                throw new ArgumentNullException(nameof(registerUserDTO));
            }

            // Check if the email is already registered
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerUserDTO.Email);
            if (existingUser != null)
            {
                throw new Exception("Email is already registered.");
            }

            // Retrieve admin password from configuration
            string adminPassword = _configuration["ADMIN_PASSWORD"] ?? throw new Exception("Admin password is missing in configuration.");

            // Check if the user wants to register as an Admin
            if (registerUserDTO.Role == "Admin")
            {
                if (string.IsNullOrEmpty(registerUserDTO.AdminPassword) || registerUserDTO.AdminPassword != adminPassword)
                {
                    throw new Exception("Invalid admin password.");
                }
            }

            // Hash the password using BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password ?? throw new ArgumentNullException(nameof(registerUserDTO.Password)));

            // Create a new user
            var user = new User
            {
                Name = registerUserDTO.Name ?? throw new ArgumentNullException(nameof(registerUserDTO.Name)),
                Email = registerUserDTO.Email ?? throw new ArgumentNullException(nameof(registerUserDTO.Email)),
                PasswordHash = passwordHash,
                Role = registerUserDTO.Role ?? "User" // Default role is "User"
            };

            // Save the user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<string> LoginUser(LoginUserDTO loginUserDTO)
        {
            if (loginUserDTO == null)
            {
                throw new ArgumentNullException(nameof(loginUserDTO));
            }

            // Find the user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUserDTO.Email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Verify the password
            if (user.PasswordHash == null || !BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.PasswordHash))
            {
                throw new Exception("Invalid password.");
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new Exception("JWT Key is missing in configuration."));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email ?? throw new Exception("User email is null.")),
                    new Claim(ClaimTypes.Role, user.Role ?? throw new Exception("User role is null."))
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiryInMinutes"] ?? throw new Exception("JWT ExpiryInMinutes is missing in configuration."))),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
       public async Task<List<UserResponseDTO>> GetAllUsers()
{
    return await _context.Users
        .Select(u => new UserResponseDTO
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role
        })
        .ToListAsync();
}

public async Task<UserResponseDTO> GetUserById(int id)
{
    var user = await _context.Users
        .Where(u => u.Id == id)
        .Select(u => new UserResponseDTO
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role
        })
        .FirstOrDefaultAsync();

    if (user == null)
    {
        throw new Exception("User not found.");
    }

    return user;
}
    }
}