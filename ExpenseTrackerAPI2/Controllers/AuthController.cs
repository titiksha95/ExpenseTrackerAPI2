using ExpenseTrackerAPI2.Data;
using ExpenseTrackerAPI2.DTOs;
using ExpenseTrackerAPI2.Interfaces;
using ExpenseTrackerAPI2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI2.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IPasswordHasher<User>
            _passwordHasher;

        private readonly IJwtService _jwtService;


        public AuthController(
            ApplicationDbContext context,
            IPasswordHasher<User> passwordHasher,
            IJwtService jwtService)
        {
            _context = context;

            _passwordHasher = passwordHasher;

            _jwtService = jwtService;
        }


        // REGISTER
        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterDto registerDto)
        {
            bool emailExists =
                await _context.Users.AnyAsync(
                    u =>
                        u.Email.ToLower() ==
                        registerDto.Email.ToLower());

            if (emailExists)
            {
                return BadRequest(new
                {
                    message =
                        "Email is already registered."
                });
            }


            User user = new User
            {
                Name = registerDto.Name,

                Email = registerDto.Email,

                CreatedAt = DateTime.UtcNow
            };


            user.PasswordHash =
                _passwordHasher.HashPassword(
                    user,
                    registerDto.Password);


            _context.Users.Add(user);

            await _context.SaveChangesAsync();


            return Ok(new
            {
                message =
                    "Registration successful.",

                userId = user.UserId,

                name = user.Name,

                email = user.Email
            });
        }


        // LOGIN
        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginDto loginDto)
        {
            User? user =
                await _context.Users
                    .FirstOrDefaultAsync(
                        u =>
                            u.Email.ToLower() ==
                            loginDto.Email.ToLower());


            if (user == null)
            {
                return Unauthorized(new
                {
                    message =
                        "Invalid email or password."
                });
            }


            PasswordVerificationResult result =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    loginDto.Password);


            if (result ==
                PasswordVerificationResult.Failed)
            {
                return Unauthorized(new
                {
                    message =
                        "Invalid email or password."
                });
            }


            string token =
                _jwtService.GenerateToken(user);


            return Ok(new
            {
                message = "Login successful.",

                token = token,

                user = new
                {
                    user.UserId,
                    user.Name,
                    user.Email
                }
            });
        }
    }
}