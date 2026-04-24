using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockTrackApi.Data;
using StockTrackApi.Models.Entities;
using StockTrackApi.Models.Entities.Dtos;

namespace StockTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public IActionResult CreateProduct([FromBody] RegisterProductDto request)
        {
            var product = new Product
            {
                ProductName = request.ProductName,
                Price = request.Price,
                Stock = request.Stock,
                Barcode = new string[] { request.Barcode },
            };
            return Ok(new { Message = "Ürün eklendi" });
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var prods = _context.Products.ToList();
            return Ok(prods);

        }
        [HttpGet("Search")]
        public IActionResult GetProductByName(string ProductName)
        {
            if (string.IsNullOrEmpty(ProductName))
            {
                return BadRequest(new { Message = "Bulmak istediğiniz ürünün adını giriniz." });
            }
            var prods = _context.Products
                 .Where(p => p.ProductName.ToLower().Contains(ProductName.ToLower()))
                 .Select(p => new ProductListDto
                 {
                     Id = p.Id,
                     ProductName = p.ProductName,
                     Price = p.Price,
                     Stock = p.Stock,
                     Explanation = p.Explanation
                 })
                 .ToList();
            if (!prods.Any())
            {
                return NotFound(new { Message = "Ürün bulunamadı" });
            }

            return Ok(prods);
        }


        [HttpGet]
        public IActionResult GetProductByBarcode(string Barcode)
        {
            var prods = _context.Products.FirstOrDefault(p => p.Barcode.Contains(Barcode));
            if (prods == null)
            {
                return NotFound(new { Message = $"'{Barcode}' barkodlu ürün bulunamadı" });
            }

            return Ok(prods);

        }

        [HttpPost("Delete")]
        public IActionResult DeleteProduct([FromBody] Guid Id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == Id);
            if (product == null)
            {
                return NotFound(new { Message = "Ürün bulunamadı" });
            }
            else
            {
                product.IsDeleted = true;
                _context.SaveChanges();
                return Ok(product);
            }

        }


        [HttpPost("Update")]
        public IActionResult UpdateProduct([FromBody] UpdateProductDto request)
        {
            //listelenen ürün üzerinden seçim yap
            var product = _context.Products.FirstOrDefault(p => p.Id == request.Id); //? 2? isdeleted kontrol edilmeli mi
            if (product == null)
            {
                return NotFound(new { Message = "Ürün bulunamadı" });
            }
            else
            {
                product.ProductName = request.ProductName;
                product.Price = request.Price;
                product.Explanation = request.Explanation;
                product.Stock = request.Stock;
                product.Barcode = request.Barcode;
                product.LastIncomingDate = request.LastIncomingDate;
                _context.SaveChanges();
                return Ok((new { Message = "Ürün güncellendi" }));
            }
        }

        [HttpPost("Sell")]
        public IActionResult SellProduct(SellProductDto request)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == request.Id); //ürün ara

            if (product == null)
            {
                return NotFound(new { Message = "Ürün bulunamadı" });
            }
            if (request.Quantitiy <= 0)
            {
                return BadRequest(new { Message = "Satış miktarı 0 veya negatif olamaz." });
            }
            if (product.Stock < request.Quantitiy)
            {
                return BadRequest(new { Message = $"Yeterli stok bulunmamaktadır. Mevcut stok: {product.Stock}" });
            }
            else
            {
                product.Stock -= request.Quantitiy;
                _context.SaveChanges();
                return Ok(new { Message = $"Ürün satışı yapıldı. Kalan stok {product.Stock}" });
            }

        }

        [HttpPost("UpdateStock")]
        public IActionResult UpdateStock(UpdateStockDto request)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == request.Id); //ürün ara
            if (product == null)
            {
                return NotFound(new { Message = "Ürün bulunamadı" });
            }
            if (request.Quantity < 0)
            {
                return BadRequest(new { Message = "Stok miktarı negatif olamaz." });
            }
            else
            {
                product.Stock += request.Quantity;
                product.LastIncomingDate = request.LastIncomingDate;
                _context.SaveChanges();
            }
            return Ok();
        }
    }
}
