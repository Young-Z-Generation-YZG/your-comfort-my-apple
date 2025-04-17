using Microsoft.AspNetCore.Mvc;

namespace YGZ.Ordering.Api.Controllers;

public class ReviewController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
