using Agenda.Domain.Entities;

namespace Agenda.Domain.Repositories
{
    public interface IContatoRepository
    {
        Task<IEnumerable<Contato>> ObterTodosAsync();
        Task<Contato> ObterPorIdAsync(int id);
        Task AdicionarAsync(Contato contato);
        Task AtualizarAsync(Contato contato);
        Task RemoverAsync(int id);
    }
}
