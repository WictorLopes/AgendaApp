using Agenda.Domain.Entities;
using Agenda.Domain.Repositories;
using Agenda.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Infrastructure.Repositories
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly AgendaDbContext _context;

        public ContatoRepository(AgendaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contato>> ObterTodosAsync()
        {
            return await _context.Contatos.ToListAsync() ?? new List<Contato>();
        }

        public async Task<Contato> ObterPorIdAsync(int id)
        {
            return await _context.Contatos.FindAsync(id);
        }

        public async Task AdicionarAsync(Contato contato)
        {
            await _context.Contatos.AddAsync(contato);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Contato contato)
        {
            _context.Contatos.Update (contato);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var contato = await _context.Contatos.FindAsync(id);
            if (contato != null)
            {
                _context.Contatos.Remove (contato);
                await _context.SaveChangesAsync();
            }
        }
    }
}
