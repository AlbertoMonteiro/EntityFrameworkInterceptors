using System.Data.Entity;

namespace EntityFrameworkInterceptors
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Contexto, Configuration>());
            using (var ctx = new Contexto())
            {
                ctx.Database.CreateIfNotExists();
            }

            using (var ctx = new Contexto())
            {
                for (int i = 0; i < 10; i++)
                    ctx.Pessoas.Add(new Pessoa { Nome = "Alberto Monteiro", Idade = i });
                ctx.SaveChanges();
            }

            using (var ctx = new Contexto())
            {
                for (int i = 10; i < 21; i++)
                    ctx.Pessoas.Add(new Pessoa { Nome = "Alberto Monteiro", Idade = i });
                ctx.SaveChanges();
            }

            /*using (var ctx = new Contexto())
            {
                for (int i = 10; i < 21; i++)
                    ctx.Pessoas.Add(new Pessoa { Nome = "Alberto Monteiro", Idade = i });
                ctx.SaveChanges();
            }*/
        }
    }
}
