using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AE.AuditPlanning.Connectivity.OsmNominatim;
using AE.AuditPlanning.Logic.Models;
using AE.AuditPlanning.Logic.ViewServiceInterfaces;
using AE.AuditPlanning.Storage;
using AE.AuditPlanning.Storage.Converter;
using AE.AuditPlanning.Storage.Entities;
using AE.AuditPlanning.Storage.Loader;
using AE.AuditPlanning.Storage.Repositories;
using AutoMapper;

namespace AE.AuditPlanning.Logic.ViewServices
{
    public class JsonLoaderViewService : IJsonLoaderViewService
    {
        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public IEnumerable<CustomerModel> LoadJson(string filePath)
        {
            JsonHelper.LoadListInRepository<Customer>(filePath);
            var customers = Repository.Current.GetList<Customer>();
            return customers.Select(Mapper.Map<Customer, CustomerModel>);
        }

        public void Save(string outputFilePath, IEnumerable<CustomerModel> customers)
        {
            JsonHelper.Save(outputFilePath, customers.Select(Mapper.Map<CustomerModel, Customer>));
        }

        public void LoadGeoCordsParallel(IEnumerable<CustomerModel> customers, Action<int> progressCallback)
        {
            var count = 0;
            var onePercent = 100.0 / customers.Count();

            var po = new ParallelOptions
            {
                MaxDegreeOfParallelism = 7
            };

            Parallel.ForEach(
                customers,
                po,
                delegate(CustomerModel model, ParallelLoopState state, long arg3)
                {
                    if (model.Latitude > 0)
                    {
                        return;
                    }

                    var cords = OsmNominatimServiceGermany.GetGeoCoordinates(model.PostalCode, model.City);
                    model.Latitude = cords.Latitude;
                    model.Longitude = cords.Longitude;

                    count++;
                    progressCallback(Convert.ToInt32(onePercent * count));
                });
        }
    }
}