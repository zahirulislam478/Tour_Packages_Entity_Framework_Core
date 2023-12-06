using Tour_Packages_and_Bookings.Models;

namespace Tour_Packages_and_Bookings.ViewModels
{
    public class GroupedData
    {
        public string Key { get; set; } = default!;
        public IEnumerable<Booking> Data { get; set; } = default!;
    }
}
