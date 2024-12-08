using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Data.Data;

namespace Tournament.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder builder )
        {
            using(var scope = builder.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var db = serviceProvider.GetRequiredService<TournamentApiContext>();

                await db.Database.MigrateAsync();

                if (await db.Game.AnyAsync() || await db.TournamentDetails.AnyAsync()) return;

                try
                {
                    var seedTournaments = SeedData.GenerateTournaments(5);
                    db.AddRange(seedTournaments);
                    db.SaveChanges();

                    var seedGames = SeedData.GenerateGames(5)
                    .Select(g => {g.TournamentId = seedTournaments.First().Id;//Kopplar alla games till en skapade Tournament 
                        return g;
                         }).ToList();

                    db.AddRange(seedGames);
                    db.SaveChanges();

                   


                    await db.SaveChangesAsync();
                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"Error while seeding data:{ex.Message}");
                    throw;

                }
            }

        }
    }
}
