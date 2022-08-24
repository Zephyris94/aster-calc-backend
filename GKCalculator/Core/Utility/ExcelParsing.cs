using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Core.Settings;
using ExcelDataReader;
using Infrastructure.Utility;
using Microsoft.Extensions.Options;
using Model;
using Model.Domain;

namespace Core.Utility
{
    public class ExcelParsing : IExcelParsing
    {
        private readonly string _excelFilePath;

        public ExcelParsing(IOptions<DataSourceOptions> dataSourceOptions)
        {
            _excelFilePath = dataSourceOptions.Value.ExcelPath;
        }

        public List<RouteModel> ParseExcelFromStream(MemoryStream ms)
        {
            var models = new List<RouteModel>();
            using (var reader = ExcelReaderFactory.CreateReader(ms))
            {
                do
                {
                    while (reader.Read()) //Each ROW
                    {
                        var source = reader.GetValue(0).ToString();
                        var destination = reader.GetValue(1).ToString();
                        var level = reader.GetValue(2)?.ToString();
                        var price = reader.GetValue(3).ToString().Replace(" ", "");
                        var type = reader.GetValue(4).ToString();
                        var convertedType = GetPathType(type);
                        var newTpModel = new RouteModel()
                        {
                            Source = new NodeModel
                            {
                                Name = source,
                                NodeType = NodeType.Source
                            },
                            Destination = new NodeModel
                            {
                                Name = convertedType == MoveType.Paradox ? $"{destination} lv.{level}" : destination,
                                NodeType = NodeType.Destination
                            },
                            Price = int.Parse(Regex.Replace(price, @"\s+", "")),
                            MoveType = convertedType
                        };
                        models.Add(newTpModel);
                    }
                } while (reader.NextResult()); //Move to NEXT SHEET
            }

            return models;
        }

        [Obsolete]
        public List<RouteModel> ParseExcelFromFile()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var models = new List<RouteModel>();
            using (var stream = File.Open(_excelFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read()) //Each ROW
                        {
                            var source = reader.GetValue(0).ToString();
                            var destination = reader.GetValue(1).ToString();
                            var level = reader.GetValue(2)?.ToString();
                            var price = reader.GetValue(3).ToString().Replace(" ","");
                            var type = reader.GetValue(4).ToString();
                            var convertedType = GetPathType(type);
                            var newTpModel = new RouteModel()
                            {
                                Source = new NodeModel
                                {
                                    Name = source,
                                    NodeType = NodeType.Source
                                },
                                Destination = new NodeModel
                                {
                                    Name = convertedType == MoveType.Paradox ? $"{destination} lv.{level}" : destination,
                                    NodeType = NodeType.Destination
                                },
                                Price = int.Parse(Regex.Replace(price, @"\s+", "")),
                                MoveType = convertedType
                            };
                            models.Add(newTpModel);
                        }
                    } while (reader.NextResult()); //Move to NEXT SHEET

                }
            }

            return models;
        }

        private static MoveType GetPathType(string str)
        {
            switch (str.ToLower())
            {
                case "gk":
                    return MoveType.GK;
                case "wyvern":
                    return MoveType.Wyvern;
                case "ship":
                    return MoveType.Ship;
                case "soe":
                    return MoveType.SoE;
                case "paradox":
                    return MoveType.Paradox;
                default:
                    return MoveType.GK;
            }
        }
    }
}
