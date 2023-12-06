using Microsoft.AspNetCore.Mvc;

namespace Tour_Packages_and_Bookings.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
