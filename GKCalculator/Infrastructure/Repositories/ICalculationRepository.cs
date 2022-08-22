using Model.Domain;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface ICalculationRepository
    {
        Task SaveCalculation(CalculationModel calculation);
    }
}
