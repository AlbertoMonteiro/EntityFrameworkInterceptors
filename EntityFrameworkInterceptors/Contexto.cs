using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;

namespace EntityFrameworkInterceptors
{
    public class Contexto : DbContext
    {
        public Contexto()
        {
            DbInterception.Add(new AuditingCommandInterceptor());
        }
        public DbSet<Pessoa> Pessoas { get; set; }
    }
}