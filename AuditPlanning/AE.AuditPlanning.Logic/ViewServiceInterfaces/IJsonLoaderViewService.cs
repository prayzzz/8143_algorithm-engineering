using System;
using System.Collections.Generic;

using AE.AuditPlanning.Logic.Models;

namespace AE.AuditPlanning.Logic.ViewServiceInterfaces
{
    public interface IJsonLoaderViewService
    {
        bool FileExists(string filePath);

        IEnumerable<CustomerModel> LoadJson(string filePath);

        void Save(string outputFilePath, IEnumerable<CustomerModel> customers);

        void LoadGeoCordsParallel(IEnumerable<CustomerModel> customers, Action<int> progressCallback);
    }
}