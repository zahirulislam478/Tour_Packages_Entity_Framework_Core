using Tour_Packages_and_Bookings.Models;

namespace Tour_Packages_and_Bookings.ViewModels 
{
    public class GroupedDataPrimitve<T>
    {
        public string Key { get; set; } = default!;
        public T Data { get; set; } = default!;
    }
}
