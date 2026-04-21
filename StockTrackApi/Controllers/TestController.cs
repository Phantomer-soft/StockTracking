using Microsoft.AspNetCore.Mvc;

namespace StockTrackApi.Controllers;

public class TestController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}