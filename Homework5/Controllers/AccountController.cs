using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Homework5.Data;
using Homework5.Models;

namespace Account.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly TokenOption tokenOption;
        private readonly AppDbContext _context;
        public AccountController(UserManager<IdentityUser> userManager,AppDbContext context, IOptions<TokenOption>options) =>
            (_userManager, tokenOption) = (userManager, options.Value);

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto login)
        {
            List<Claim> claims = new List<Claim>();
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null) throw new Exception("");

            var result = await _userManager.CheckPasswordAsync(user, login.Password);
            if (result)
            {

                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                claims.Add(new Claim(ClaimTypes.Name, user.Email));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                var token = GetToken(claims);

                var handler = new JwtSecurityTokenHandler();
                string jwt = handler.WriteToken(token);

                return Ok(new
                {
                    token = jwt,
                    expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            if (_userManager.Users.Any(u => u.Email == request.Email))
            {
                return BadRequest("User already exists.");
            };

            var user = new IdentityUser { Email=request.Email ,UserName=request.UserName};

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(x=>x));
            }

            return Ok("User Created");
        }



        private JwtSecurityToken GetToken(List<Claim> claims)
        {

            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOption.Key));

            var token = new JwtSecurityToken(

                 signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256),
                 issuer: tokenOption.Issuer,
                 audience: tokenOption.Audience,
                 expires: DateTime.Now.AddMinutes(tokenOption.Expiration),
                 claims: claims
                );

            return token;

        }

    }
}
