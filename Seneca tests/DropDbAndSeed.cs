using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seneca_tests
{
    //class DropDbAndSeed( DbContext TContext, DbMigrationsConfiguration TMigrationConfiguration )
    public class DropDbAndSeed<TContext, TMigrationsConfiguration> : IDatabaseInitializer<TContext>

where TContext : DbContext

where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new()
    {
        public void InitializeDatabase(TContext context)
        {
            var step1 = new DropCreateDatabaseAlways<TContext>();
            step1.InitializeDatabase(context);

            var step2 = new MigrateDatabaseToLatestVersion<TContext, TMigrationsConfiguration>();
            step2.InitializeDatabase(context);
        }
    }
}
