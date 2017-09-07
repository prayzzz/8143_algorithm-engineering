using System.Collections.Generic;
using System.IO;
using System.Linq;

using AE.AuditPlanning.Logic.Models;
using AE.AuditPlanning.Logic.ViewServiceInterfaces;
using AE.AuditPlanning.Storage.Converter;
using AE.AuditPlanning.Storage.Entities;
using AE.AuditPlanning.Storage.Loader;
using AutoMapper;

namespace AE.AuditPlanning.Logic.ViewServices
{
    public class CsvConvertViewService : ICsvConvertViewService
    {
        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public IEnumerable<CustomerModel> LoadCsv(string filePath, char seperator)
        {
            return CustomerLoader.LoadCsv(filePath, seperator).Select(Mapper.Map<Customer, CustomerModel>);
        }

        public void Save(string outputFilePath, IEnumerable<CustomerModel> customers)
        {
            JsonHelper.Save(outputFilePath, customers.Select(Mapper.Map<CustomerModel, Customer>));
        }
    }
}