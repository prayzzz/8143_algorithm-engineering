using System.Collections.Generic;

using AE.AuditPlanning.Logic.Models;

namespace AE.AuditPlanning.Logic.ViewServiceInterfaces
{
    public interface ICsvConvertViewService
    {
        bool FileExists(string filePath);

        IEnumerable<CustomerModel> LoadCsv(string filePath, char seperator);

        void Save(string outputFilePath, IEnumerable<CustomerModel> customers);
    }
}