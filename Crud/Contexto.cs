using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;

namespace Crud
{
    public class Contexto : DbContext
    {
        public DbSet<Pessoa> Pessoas { get; set; }
    }
}