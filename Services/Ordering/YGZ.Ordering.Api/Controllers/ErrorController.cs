using Microsoft.AspNetCore.Mvc;

namespace YGZ.Ordering.Api.Controllers;

public class ErrorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
