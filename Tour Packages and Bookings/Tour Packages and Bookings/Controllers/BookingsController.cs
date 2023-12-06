using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tour_Packages_and_Bookings.Models;

namespace Tour_Packages_and_Bookings.Controllers
{
    public class BookingsController : Controller
    {
        private readonly TourDbContext db;
        public BookingsController(TourDbContext db) 
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View(); 
        }
        public IActionResult Create(int id)
        {
            ViewBag.TourPackageId = id;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Booking model) 
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlInterpolated($"EXEC InsertBooking {model.TravelerName},{model.PhoneNumber},{model.NumberOfTravelers}, {model.BookingDate}, {model.BookingStatus} , {model.TourPackageId}");
                return RedirectToAction("Index", "TourPackages");
            }
            ViewBag.TourPackages = db.TourPackages.ToList();
            return View(model);
        }
        public IActionResult Edit(int id)
        {
            var data = db.Bookings.FirstOrDefault(x => x.BookingId == id);
            if (data == null) { return NotFound(); }
            ViewBag.TourPackages = db.TourPackages.ToList();
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(Booking model)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlInterpolated($"EXEC UpdateBooking {model.BookingId}, {model.TravelerName},{model.PhoneNumber},{model.NumberOfTravelers}, {model.BookingDate}, {model.BookingStatus} , {model.TourPackageId}");
                return RedirectToAction("Index", "TourPackages");
            }
            ViewBag.TourPackages = db.TourPackages.ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            db.Database.ExecuteSqlInterpolated($"EXEC DeleteBooking {id}");
            return Json(new { success = true, id });
        }
    }
}
