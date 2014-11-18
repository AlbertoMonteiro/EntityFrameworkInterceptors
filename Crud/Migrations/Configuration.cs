using System.Data.Entity.Migrations;

namespace Crud.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Contexto>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    }
}
