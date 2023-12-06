using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Tour_Packages_and_Bookings.Models
{
    public enum PackageType
    {
        Standard =1,
        Premium,
        Deluxe,
    }
    public class TourPackage
    {
        public int TourPackageId { get; set; }

        [Required,StringLength(100)]
        public string PackageName { get; set; } = default!;

        [Required, StringLength(100)]
        public string Description { get; set; } = default!;

        public int Duration { get; set; }

        [Required, StringLength(100)]
        public string Destinations { get; set; } = default!;

        [Required,Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Required, StringLength(100)]
        public string? Picture { get; set; } = default!;

        [Required,EnumDataType(typeof(PackageType))]
        public PackageType? PackageType { get; set; } 

        public bool? IsActive { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
    public class Booking 
    {
        public int BookingId { get; set; }

        [Required,StringLength(255)]
        public string TravelerName { get; set; } = default!;

        [Required,StringLength(100)]
        public string PhoneNumber { get; set; } = default!;

        public int NumberOfTravelers { get; set; }

        [Required,Column(TypeName = "date")]
        public DateTime? BookingDate { get; set; }

        [Required, StringLength(100)]
        public string BookingStatus { get; set; } = default!;

        [Required, ForeignKey("TourPackage")]
        public int TourPackageId { get; set; }

        public virtual TourPackage? TourPackage { get; set; }
    }
    public class TourDbContext : DbContext
    {
        public TourDbContext(DbContextOptions<TourDbContext> options) : base(options) { } 

        public DbSet<TourPackage> TourPackages { get; set; } = default!; 

        public DbSet<Booking> Bookings { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Random random = new Random();

            for (int i = 1; i <= 5; i++)
            {
                modelBuilder.Entity<TourPackage>().HasData(
                new TourPackage
                {
                    TourPackageId = i,
                    PackageName = "Package " + i,
                    Description = "Description for Package " + i,
                    Duration = random.Next(3, 10),
                    Destinations = "Destination " + i,
                    Price = random.Next(1000, 5000) * 1.00M,
                    Picture = "package_" + i + ".jpg",
                    PackageType = (PackageType)random.Next(1, 4),
                    IsActive = true
                });
            }

            for (int i = 1; i <= 8; i++)
            {
                modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    BookingId = i,
                    TravelerName = "Traveler " + i,
                    PhoneNumber = "123-456-789" + i,
                    NumberOfTravelers = random.Next(1, 5),
                    BookingDate = DateTime.Now.AddDays(-random.Next(1, 30)),
                    BookingStatus = (i % 2 == 0) ? "Confirmed" : "Pending",
                    TourPackageId = i % 5 == 0 ? 5 : i % 5
                });
            }
        }
    }
}
