using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockTrackApi.Data;
using StockTrackApi.Helpers;
using StockTrackApi.Models.Entities;
using StockTrackApi.Models.Entities.Dtos;

namespace StockTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        AppDbContext _context;
        JwtTokenHelper _jwtTokenHelper;
        IConfiguration _configuration;

        public AuthController(AppDbContext context, JwtTokenHelper jwtTokenHelper, IConfiguration configuration)
        {
            _context = context;
            _jwtTokenHelper = jwtTokenHelper;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult CreateCompany([FromBody] RegisterCompanyDto request)
        {
            var user = new Company
            {
                Name = request.CompanyName,
                Mail=request.Mail,
                Password=request.Password, // password düz string olarak tutulmaz şimdilik dursun daha sonra hashleriz 
                PhoneNumber=request.PhoneNumber,
            };

            _context.Companies.Add(user);
            _context.SaveChanges();

            return Ok("Kayıt başarılı");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginCompanyDto request)
        {
            var company = _context.Companies
                .FirstOrDefault(c=>c.Mail==request.Mail &&  c.Password==request.Password && c.Active);
            if (company == null)
            {
                return BadRequest(new { Message = "E posta veya şifre hatalı" });
            }
            return Ok(new
            {
                Message="Giriş başarılı",
                Token=_jwtTokenHelper.CreateAccessToken(_configuration,company)
            });
        }
    }
}
