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
        private Funcs _functions = new Funcs();

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
                Explanation = request.Explanation
            };
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(new { Message = "Ürün eklendi" });
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            var prods = _context.Products.ToList();
            var dto = prods.Select(p => new ProductListDto()
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                Barcode = p.Barcode,
                Explanation = p.Explanation,
                Stock = p.Stock
            }).ToList();
          
            return Ok(dto);

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


        [HttpGet("GetProductByBarcode")]
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
            var product = _functions.FindProduct(Id,_context);
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
            var product = _functions.FindProduct(request.Id,_context); //? 2? isdeleted kontrol edilmeli mi
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
            var product = _functions.FindProduct(request.Id,_context); //ürün ara
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
            var product = _functions.FindProduct(request.Id,_context); //ürün ara
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

public class Funcs
{
    private readonly AppDbContext _context;

    public Funcs(AppDbContext context)
    {
        _context = context;
    }

    public Funcs()
    {
    }

    public Product? FindProduct(Guid id,AppDbContext context)
    {
        if (context == null) throw new Exception("Database context is not initialized.");

        var product = context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return null; 
        }
        return product;
    }
}
