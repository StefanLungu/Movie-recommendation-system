using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RatingsMicroservice.Data;
using RatingsMicroservice.Resources;
using RatingsMicroservice.Services;
using System.IO;

namespace RatingsMicroservice.Helpers
{
    public static class HostExtensions
    {
        public static IHost Seed(this IHost host)
        {
            if (!File.Exists(Messages.Database))
            {
                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;
                DataContext context = services.GetService<DataContext>();
                ICsvParser parser = services.GetService<ICsvParser>();

                context.Database.Migrate();

                Seeder seeder = new Seeder(context, parser);
                seeder.SeedRatings();
            }

            return host;
        }
    }
}
