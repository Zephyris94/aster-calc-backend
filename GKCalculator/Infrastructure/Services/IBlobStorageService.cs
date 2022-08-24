using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IBlobStorageService
    {
        Task DownloadToStream(MemoryStream ms);
    }
}
