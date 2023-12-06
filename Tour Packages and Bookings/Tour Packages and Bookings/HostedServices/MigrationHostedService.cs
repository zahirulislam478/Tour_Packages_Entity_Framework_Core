namespace Tour_Packages_and_Bookings.HostedServices 
{
    public class MigrationHostedService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        public MigrationHostedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var svc = scope.ServiceProvider.GetRequiredService<ApplyMigrationService>();
            await svc.ApplyMigrationAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
