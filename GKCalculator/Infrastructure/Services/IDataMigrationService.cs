using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IDataMigrationService
    {
        void SeedData();

        Task SeedDataAsync();
    }
}
