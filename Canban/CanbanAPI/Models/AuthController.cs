using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CanbanAPI.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static Uzytkownicy user = new Uzytkownicy();
        private readonly IConfiguration _configuration;
        private readonly CanbanDBContext _context;

        public AuthController(IConfiguration configuration, CanbanDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Uzytkownicy>> Register(UserRegister userRegRequest)
        {

            if (!IsAnyNullOrEmpty(userRegRequest))
            {
                Uzytkownicy user = new Uzytkownicy();
                CreatePasswordHash(userRegRequest.password, out byte[] UserPasswordHash, out byte[] UserPasswordSalt);

                string birthDate = userRegRequest.birthDate;
                DateTime date = DateTime.Parse(birthDate);

                if (!UserExists(userRegRequest.login) && userRegRequest.login.Length <= 20)
                {
                    user.NazwaUzytkownik = userRegRequest.login;
                    user.HasloHashUzytkownik = UserPasswordHash;
                    user.SaltHasloUzytkownik = UserPasswordSalt;
                    user.DataRejestracjaUzytkownik = date;
                    user.EmailUzytkownik = userRegRequest.email;
                    _context.Uzytkownicies.Add(user);
                    await _context.SaveChangesAsync();
                    return Ok("Konto zostało pomyślnie utworzone");
                }
                else if (userRegRequest.login.Length > 20)
                {
                    return BadRequest("User login should have less or equal 20 chars");
                }
                else
                {
                    return BadRequest("User with this login exist!");
                }
            }
            else if (IsAnyNullOrEmpty(userRegRequest))
            {
                return BadRequest("Please complete all fields");
            }
            else
            {
                return BadRequest("Something went wrong");
            }


        }

        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<Uzytkownicy>>> GetUser(string username)
        {
            var user = await _context.Uzytkownicies.Where(x => x.NazwaUzytkownik == username).ToListAsync();

            if (user.Count() == 0)
            {
                return BadRequest("Wrong password or login");
            }

            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDTO UserLogRequest)
        {
            var userGet = await GetUser(UserLogRequest.login);
            if (userGet.Value != null)
            {
                var accUser = userGet.Value.First();

                if (accUser.NazwaUzytkownik != UserLogRequest.login)
                {
                    return BadRequest("Niepoprawne dane logowania");
                }

                if (!VerifyPasswordHash(UserLogRequest.password, accUser.HasloHashUzytkownik, accUser.SaltHasloUzytkownik))
                {
                    return BadRequest("Niepoprawne dane logowania");
                }
                string token = CreateToken(accUser);

                return token;
            }
            else
            {
                return BadRequest("Niepoprawne dane logowania");
            }
  
        }

        private string CreateToken(Uzytkownicy user)
        {

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.NazwaUzytkownik));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(50),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] UserPasswordHash, out byte[] UserPasswordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                UserPasswordSalt = hmac.Key;
                UserPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] UserPasswordHash, byte[] UserPasswordSalt)
        {
            using (var hmac = new HMACSHA512(UserPasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(UserPasswordHash);
            }
        }

        private bool UserExists(string nickname)
        {
            return _context.Uzytkownicies.Any(e => e.NazwaUzytkownik == nickname);
        }

        bool IsAnyNullOrEmpty(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
