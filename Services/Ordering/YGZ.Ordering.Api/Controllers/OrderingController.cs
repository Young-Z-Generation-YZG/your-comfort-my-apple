using Microsoft.AspNetCore.Mvc;

namespace YGZ.Ordering.Api.Controllers;

public class OrderingController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
