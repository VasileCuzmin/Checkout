using System.Threading.Tasks;

namespace Checkout.Migrations
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var migrator = new DatabaseMigrator();
            await migrator.MigrateToLatestVersion();
        }
    }
}