using System.Collections.Generic;
using System.IO;
using System.Linq;

using AE.AuditPlanning.Storage.Entities;

namespace AE.AuditPlanning.Storage.Loader
{
    public static class CustomerLoader
    {
        public static IEnumerable<Customer> LoadCsv(string inputFilePath, char seperator)
        {
            if (!File.Exists(inputFilePath))
            {
                return new List<Customer>();
            }

            var crlf = new[] { '\n', '\r' };
            TextReader tr = File.OpenText(inputFilePath);
            var fileLines = tr.ReadToEnd().Split(crlf);

            var list = new List<Customer>();
            foreach (var splittedLine in fileLines.Where(x => !string.IsNullOrEmpty(x)).Select(line => line.Split(seperator)))
            {
                list.Add(new Customer { PostalCode = int.Parse(splittedLine[0]), City = splittedLine[1] });
            }

            return list;
        }
    }
}