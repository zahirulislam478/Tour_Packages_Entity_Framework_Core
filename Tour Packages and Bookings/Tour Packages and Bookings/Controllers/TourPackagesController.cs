using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using Tour_Packages_and_Bookings.Models;
using Tour_Packages_and_Bookings.ViewModels;
using X.PagedList;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tour_Packages_and_Bookings.Controllers
{
    public class TourPackagesController : Controller
    {
        private readonly TourDbContext db;
        private readonly IWebHostEnvironment env;
        public TourPackagesController(TourDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public async Task<IActionResult> Index(int pg = 1)
        {
            return View(await db.TourPackages.OrderBy(x => x.TourPackageId).Include(x => x.Bookings).ToPagedListAsync(pg, 5));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TourPackageInputModel model)
        {
            if (ModelState.IsValid)
            {
                await db.Database.ExecuteSqlInterpolatedAsync($"EXEC InsertTour {model.PackageName},{model.Description},{model.Duration}, {model.Destinations}, {model.Price},{model.Picture},{model.PackageType}, {(model.IsActive ? "1" : "0")}");
                var id = await db.TourPackages.MaxAsync(x => x.TourPackageId);
                foreach (var s in model.Bookings)
                {
                    await db.Database.ExecuteSqlInterpolatedAsync($"EXEC InsertBooking {s.TravelerName}, {s.PhoneNumber},{s.NumberOfTravelers}, {s.BookingDate},{s.BookingStatus}, {id}");
                }
                return Json(new { success = true });
            }
            return Json(new { success = true });
        }

        public IActionResult GetBookingsForm()
        { 
            return PartialView("_BookingsForm");
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            string ext = Path.GetExtension(file.FileName);
            string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
            string savePath = Path.Combine(env.WebRootPath, "Pictures", fileName);
            FileStream fs = new FileStream(savePath, FileMode.Create);
            await file.CopyToAsync(fs);
            fs.Close();
            return Json(new { name = fileName });
        }
        public async Task<IActionResult> Edit(int id)
        {
            var data = await db.TourPackages.FirstOrDefaultAsync(x => x.TourPackageId == id);
            if (data == null)
            {
                return NotFound();
            }
            var model = new TourPackageEditModel
            {
                TourPackageId = data.TourPackageId,
                PackageName = data.PackageName,
                Description = data.Description,
                Duration = data.Duration,
                Destinations = data.Destinations,
                Price = data.Price,
                PackageType = data.PackageType ?? PackageType.Premium,
                IsActive = data.IsActive ?? false
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TourPackageEditModel model)
        {
            if (ModelState.IsValid) 
            {
                var tourPackage = await db.TourPackages.FirstOrDefaultAsync(x => x.TourPackageId == model.TourPackageId);
                if (tourPackage == null) return NotFound();
                tourPackage.TourPackageId = model.TourPackageId;
                tourPackage.PackageName = model.PackageName;
                tourPackage.Description = model.Description;
                tourPackage.Duration = model.Duration;
                tourPackage.Destinations = model.Destinations;
                tourPackage.Price = model.Price;
                tourPackage.PackageType = model.PackageType;
                tourPackage.IsActive = model.IsActive;

                if (model.Picture != null)
                {
                    string ext = Path.GetExtension(model.Picture.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(env.WebRootPath, "Pictures", fileName);
                    FileStream fs = new FileStream(savePath, FileMode.Create);
                    await model.Picture.CopyToAsync(fs);
                    tourPackage.Picture = fileName;
                    fs.Close();
                }
                db.Database.ExecuteSqlInterpolated($"EXEC UpdateTour {tourPackage.TourPackageId}, {tourPackage.PackageName}, {tourPackage.Description}, {model.Duration}, {tourPackage.Destinations}, {tourPackage.Price}, {tourPackage.Picture}, {(int)tourPackage.PackageType}, {(model.IsActive ? "1" : "0")}");
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            db.Database.ExecuteSqlInterpolated($"EXEC DeleteTour {id}");
            return Json(new { success = true, id });
        }

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || db.TourPackages == null)
        //    {
        //        return NotFound();
        //    }
        //    var t = await db.TourPackages
        //        .FirstOrDefaultAsync(m => m.TourPackageId == id);
        //    if (t == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(t);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    if (db.TourPackages == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Properties'  is null.");
        //    }
        //    var tour = await db.TourPackages.FindAsync(id);
        //    if (tour != null)
        //    {
        //        db.TourPackages.Remove(tour);
        //    }
        //    await db.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> Aggregates()
        {
            var data = await db.Bookings.Include(x => x.TourPackage)
                .ToListAsync();
            return View(data);
        }

        public IActionResult Grouping()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Grouping(string groupby)
        {
            if (groupby == "packagename")
            {
                var data = db.Bookings.Include(x => x.TourPackage)
               .ToList()
               .GroupBy(x => x.TourPackage?.PackageName)
               .Select(g => new GroupedData { Key = g.Key, Data = g })
               .ToList();

                return View("GroupingResult", data);
            }

            if (groupby == "year month")
            {
                var data = db.Bookings.Include(x => x.TourPackage)
                    .OrderByDescending(x => x.BookingDate)
               .ToList()
               .GroupBy(x => $"{x.BookingDate:MMM, yyyy}")
               .Select(g => new GroupedData { Key = g.Key, Data = g })
               .ToList();

                return View("GroupingResult", data);
            }

            if (groupby == "count")
            {
                var data = db.Bookings.Include(x => x.TourPackage)
                    .OrderByDescending(x => x.BookingDate)
               .ToList()
               .GroupBy(x => x.TourPackage?.PackageName)
               .Select(g => new GroupedDataPrimitve<int> { Key = g.Key, Data = g.Count() })
               .ToList();

                return View("GroupingResultPrimitive", data);
            }

            return RedirectToAction("Grouping");
        }
    }
}
