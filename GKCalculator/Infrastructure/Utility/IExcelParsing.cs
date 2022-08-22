using System.Collections.Generic;
using Model.Domain;

namespace Infrastructure.Utility
{
    public interface IExcelParsing
    {
        List<RouteModel> ParseExcel();
    }
}
