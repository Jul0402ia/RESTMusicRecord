using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RESTMusicRecord.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _configuration;

        // Constructor som giver adgang til appsettings.json
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Login metode som laver en JWT token
        [HttpPost("login")]
        public IActionResult Login([FromBody] Login login)
        {
            // Tjekker om login-data mangler
            if (login == null || login.Username == "" || login.Password == "")
            {
                return BadRequest("Brugernavn og password skal udfyldes.");
            }

            // Simpelt login
            if (login.Username != "admin" || login.Password != "1234")
            {
                return Unauthorized("Forkert brugernavn eller password.");
            }

            // Henter Jwt-sektionen fra appsettings.json
            IConfigurationSection jwtSettings = _configuration.GetSection("Jwt");

            // Henter hemmelig nøgle fra appsettings.json
            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            // Opretter signering med hemmelig nøgle
            SigningCredentials credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            // Opretter claims (info om brugeren)
            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, login.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            // Opretter token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            // Gør token til string
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Sender token tilbage
            return Ok(new
            {
                token = tokenString
            });
        }
    }
}