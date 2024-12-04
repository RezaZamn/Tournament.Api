using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Bogus;

namespace Tournament.Data.Data;


    public static class SeedData
{ 
    public static IEnumerable<Game> GenerateGames(int nrOfGames)
    {
        var faker = new Faker<Game>("sv").Rules((f, c) =>
        {
            c.Title = f.Lorem.Word();
            c.Time = f.Date.Future(1);
            c.TournamentId = f.Random.Int(1, 10);          

        });

        return faker.Generate(nrOfGames);

    }

    public static IEnumerable<TournamentDetails> GenerateTournaments(int nrOfTournaments)
    {
        var faker = new Faker<TournamentDetails>("sv").Rules((f, c) =>
        {
            c.Title = f.Lorem.Word();
            c.StartDate = f.Date.Future(1); //Max ett år framåt
            c.Games = GenerateGames(f.Random.Int(min: 2, max: 10)).ToList(); //Konverterar till list
        });

        return faker.Generate(nrOfTournaments);
    }
    
}
