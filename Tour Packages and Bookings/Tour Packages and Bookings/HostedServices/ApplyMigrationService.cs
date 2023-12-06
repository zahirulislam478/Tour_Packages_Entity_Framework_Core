using Microsoft.EntityFrameworkCore;
using Tour_Packages_and_Bookings.Models;

namespace Tour_Packages_and_Bookings.HostedServices
{
    public class ApplyMigrationService
    {
        private readonly TourDbContext db;
        public ApplyMigrationService(TourDbContext db)
        {
            this.db = db;
        }
        public async Task ApplyMigrationAsync()
        {
            if((await db.Database.GetPendingMigrationsAsync()).Any())
            {
                await db.Database.MigrateAsync();
            }
        }
    }
}
