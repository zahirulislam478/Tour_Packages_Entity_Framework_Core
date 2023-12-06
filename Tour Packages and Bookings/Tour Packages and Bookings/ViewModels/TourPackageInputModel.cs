using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tour_Packages_and_Bookings.Models;

namespace Tour_Packages_and_Bookings.ViewModels
{
    public class TourPackageInputModel
    {
        public int TourPackageId { get; set; }

        [Required, StringLength(100)]
        public string PackageName { get; set; } = default!;

        [Required, StringLength(100)]
        public string Description { get; set; } = default!;

        public int Duration { get; set; }

        [Required, StringLength(100)]
        public string Destinations { get; set; } = default!;

        [Required, Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Required, StringLength(40)]
        public string Picture { get; set; } = default!;

        [Required, EnumDataType(typeof(PackageType))]
        public PackageType PackageType { get; set; }

        public bool IsActive { get; set; }

        public virtual List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
