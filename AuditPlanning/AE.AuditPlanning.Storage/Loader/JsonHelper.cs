using System.Collections.Generic;
using System.IO;
using AE.AuditPlanning.Storage.Entities;
using AE.AuditPlanning.Storage.Repositories;
using Newtonsoft.Json;

namespace AE.AuditPlanning.Storage.Loader
{
    public static class JsonHelper
    {
        public static void Save<T>(string outputFilePath, T data)
        {
            using (var file = File.CreateText(outputFilePath))
            {
                var serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
                serializer.Formatting = Formatting.Indented;

                serializer.Serialize(file, data);
            }
        }

        public static void LoadListInRepository<T>(string filePath) where T : Entity
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            using (var file = File.OpenText(filePath))
            {
                var serializer = new JsonSerializer();
                var data = (IEnumerable<T>)serializer.Deserialize(file, typeof(IEnumerable<T>));

                Repository.Current.Clear<T>();
                foreach (var d in data)
                {
                    Repository.Current.Add(d);
                }
            }
        }

        public static IEnumerable<T> LoadList<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            using (var file = File.OpenText(filePath))
            {
                var serializer = new JsonSerializer();
                return (IEnumerable<T>)serializer.Deserialize(file, typeof(IEnumerable<T>));
            }
        }
    }
}