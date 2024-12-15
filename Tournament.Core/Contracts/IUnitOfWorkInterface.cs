using Tournament.Api.Games.Core.Repositories;

namespace Tournament.Api.Tournament.Core.Repositories
{
    public interface IUnitOfWorkInterface
    {
        ITournamentRepository Tournaments { get; }
        IGameRepository Games { get; }

        Task CompleteAsync();
    }
}
