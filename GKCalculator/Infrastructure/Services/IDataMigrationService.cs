using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IDataMigrationService
    {
        Task SeedData();
    }
}
