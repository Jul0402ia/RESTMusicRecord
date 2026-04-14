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
        // Login metode som laver en JWT token
        [HttpPost("login")]
        public ActionResult Login([FromBody] Login login)
        {
            // Tjekker om login-data mangler
            if (login == null || login.Username == null || login.Password == null)
            {
                return BadRequest("Brugernavn og password skal udfyldes.");
            }

            // Simpelt login
            if (login.Username != "admin" || login.Password != "1234")
            {
                return Unauthorized("Forkert brugernavn eller password.");
            }

            // Opretter claims (info om brugeren)
            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            // Hemmelig nøgle til token
            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("THIS_IS_A_SECRET_KEY_12345678901234567890"));

            // Signering
            SigningCredentials credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            // Opretter token
            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
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