using System.Collections.Generic;
using System.IO;
using Model.Domain;

namespace Infrastructure.Utility
{
    public interface IExcelParsing
    {
        List<RouteModel> ParseExcelFromStream(MemoryStream ms);

        List<RouteModel> ParseExcelFromFile();
    }
}
