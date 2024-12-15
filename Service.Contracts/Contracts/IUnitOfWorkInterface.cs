using Tournament.Api.Games.Core.Repositories;
using Tournament.Api.Tournament.Core.Repositories;

namespace Tournament.Services.Contracts
{
    public interface IUnitOfWorkInterface
    {
        ITournamentRepository Tournaments { get; }
        IGameRepository Games { get; }

        Task CompleteAsync();
    }
}
