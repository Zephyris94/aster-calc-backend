using System;

namespace Core.Settings
{
    public class DataSourceOptions
    {
        public string BlobContainerName { get; set; }

        public string DataSourceFile { get; set; }

        public bool UseCache { get; set; }

        [Obsolete]
        public string ExcelPath { get; set; }
    }
}
