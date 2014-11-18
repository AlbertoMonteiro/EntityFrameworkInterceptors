using System.Data.Entity.Migrations;

namespace EntityFrameworkInterceptors.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Contexto>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    }
}
