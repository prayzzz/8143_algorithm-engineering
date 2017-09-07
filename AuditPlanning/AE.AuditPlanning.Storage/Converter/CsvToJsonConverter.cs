using System;
using System.Collections.Generic;
using System.IO;
using AE.AuditPlanning.Storage.Entities;
using Newtonsoft.Json;

namespace AE.AuditPlanning.Storage.Converter
{
    public static class CsvToJsonConverter
    {
        public static void Convert(string inputFilePath, string outputFilePath)
        {
            Convert(inputFilePath, outputFilePath, i => { });
        }

        public static void Convert(string inputFilePath, string outputFilePath, Action<int> progressCallback)
        {
            if (!File.Exists(inputFilePath))
            {
                return;
            }

            var crlf = new[] { '\n', '\r' };
            TextReader tr = File.OpenText(inputFilePath);
            var fileLines = tr.ReadToEnd().Split(crlf);

            var onePercent = fileLines.Length / 100;
            var customers = new List<Customer>();

            for (var index = 0; index < fileLines.Length; index++)
            {
                var line = fileLines[index];

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var splittedLine = line.Split(',');
                customers.Add(new Customer { PostalCode = int.Parse(splittedLine[0]), City = splittedLine[1] });

                progressCallback(index / onePercent);
            }

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);
            File.WriteAllText(outputFilePath, json);
        }
    }
}