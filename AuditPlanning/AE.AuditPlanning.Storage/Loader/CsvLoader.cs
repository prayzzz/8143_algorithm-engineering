using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AE.AuditPlanning.Storage.Loader
{
    public static class CsvLoader
    {
        public static IEnumerable<T> LoadCsv<T>(string inputFilePath, char seperator, Action<T, string[]> mapping) where T : new()
        {
            if (!File.Exists(inputFilePath))
            {
                return new List<T>();
            }

            var crlf = new[] { '\n', '\r' };
            TextReader tr = File.OpenText(inputFilePath);
            var fileLines = tr.ReadToEnd().Split(crlf);

            var list = new List<T>();
            foreach (var splittedLine in fileLines.Where(x => !string.IsNullOrEmpty(x)).Select(line => line.Split(seperator)))
            {
                var obj = new T();
                list.Add(obj);

                mapping.Invoke(obj, splittedLine);
            }

            return list;
        }
    }
}