using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DemoApp.Data;
using DemoApp.DTOs;
using DemoApp.Entities;
using DemoApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            this._tokenService = tokenService;
            this._context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            // Check unique username
            if (await UserExists(registerDTO.UserName))
            {
                return BadRequest("Username is taken.");
            }

            // Encrypted Password Hash Generator
            using var hmac = new HMACSHA512();

            // Creating User
            var user = new User
            {
                UserName = registerDTO.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };

            // Transaction for saving User
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var auth_user = await _context.Users.SingleOrDefaultAsync(user => user.UserName == loginDTO.UserName);
            if (auth_user == null)
            {
                return Unauthorized("Invalid Username");
            }

            using var hmac = new HMACSHA512(auth_user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != auth_user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }

            return new UserDTO
            {
                UserName = auth_user.UserName,
                Token = _tokenService.CreateToken(auth_user)
            };

        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(user => user.UserName == username.ToLower());
        }
    }
}