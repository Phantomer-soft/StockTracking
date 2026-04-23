using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockTrackApi.Data;
using StockTrackApi.Models.Entities;
using StockTrackApi.Models.Entities.Dtos;

namespace StockTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        AppDbContext context;

        public AuthController(AppDbContext _context)
        {
            context = _context;
        }

        [HttpPost]
        public IActionResult CreateCompany([FromBody] CompanyRegisterDto request)
        {
            var User = new Company
            {
                CompanyName = request.CompanyName,
                Mail=request.Mail,
                Password=request.Password,
                PhoneNumber=request.PhoneNumber,
            };

            context.Companies.Add(User);
            context.SaveChanges();

            return Ok("Kayıt başarılı");
        }

        public IActionResult Login([FromBody] CompanyLoginDto request)
        {
            var Company=context.Companies.FirstOrDefault(c=>c.Mail==request.Mail);
            if (Company == null)
            {
                return BadRequest(new { Message = "Bu mail adresine kayıtlı kullanıcı bulunamadı" });
            }
            if(Company.Password!=request.Password)
            {
                return BadRequest(new {Message="Şifre hatalı"});
            }
            if (!Company.Active)
            {
                return BadRequest(new { Message = "Hesap bulunamadı!" });
            }
           
            return Ok(new
            {
                Message="Giriş başarılı",
                CompanyId=Company.Id,
                CompanyName=Company.CompanyName
            });
        }
    }
}
