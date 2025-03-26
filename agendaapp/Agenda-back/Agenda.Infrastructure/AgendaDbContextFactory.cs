using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Agenda.Infrastructure.Data;

namespace Agenda.Infrastructure.Factories
{
    public class AgendaDbContextFactory : IDesignTimeDbContextFactory<AgendaDbContext>
    {
        public AgendaDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AgendaDbContext>();
            
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=AgendaDb;Username=postgres;Password=All0108ana@123");

            return new AgendaDbContext(optionsBuilder.Options);
        }
    }
}
