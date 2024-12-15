using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tournament.Data.Data;
using Tournament.Api.Extensions;
using Tournament.Api.Tournament.Core.Repositories;
using Tournament.Api.Tournament.Data.Repositories;
using Tournament.Api.Games.Core.Repositories;
using Tournament.Api.Games.Data.Repositories;
using Tournament.Services.Contracts;
using Tournament.Services;


namespace Tournament.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TournamentApiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TournamentApiContext") ?? throw new InvalidOperationException("Connection string 'TournamentApiContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllers(opt => opt.ReturnHttpNotAcceptable = true)
                .AddNewtonsoftJson()
                .AddXmlDataContractSerializerFormatters();


            builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddScoped<IUnitOfWorkInterface, UnitOfWorkClass>();
            builder.Services.AddAutoMapper(typeof(TournamentMappings));

            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddScoped<ITournamentService, TournamentService>();
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await app.SeedDataAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                await app.SeedDataAsync();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
