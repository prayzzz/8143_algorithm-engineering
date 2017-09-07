using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AE.AuditPlanning.Logic.Models;
using AE.AuditPlanning.Logic.ViewServiceInterfaces;
using AE.AuditPlanning.Storage;
using AE.AuditPlanning.Storage.Entities;
using AE.AuditPlanning.Storage.Loader;
using AE.AuditPlanning.Storage.Repositories;
using AutoMapper;

namespace AE.AuditPlanning.Logic.ViewServices
{
    public class GeoLocationEditorViewService : IGeoLocationEditorViewService
    {
        public IEnumerable<GeoLocationModel> GetGeoLocations()
        {
            return GeoLocationRepository.Current.GetList().Select(Mapper.Map<GeoLocationModel>);
        }

        public bool Delete(GeoLocationModel model)
        {
            var location = Mapper.Map<GeoLocation>(model);
            GeoLocationRepository.Current.Delete(location);

            return true;
        }

        public void LoadNewGeoLocationsFromFile(string path, string seperator)
        {
            this.LoadNewGeoLocationsFromFile(path, seperator, x => { });
        }

        public void LoadNewGeoLocationsFromFile(string path, string seperator, Action<int> callback)
        {
            Action<GeoLocationModel, string[]> mapping = (g, s) =>
            {
                g.PostalCode = int.Parse(s[0]);
                g.City = s[1];
            };

            var locationsToAdd = CsvLoader.LoadCsv(path, seperator[0], mapping).ToList();
            locationsToAdd = locationsToAdd.GroupBy(x => x.PostalCode).Distinct().SelectMany(x => x).ToList();

            var processedItems = 0;
            var totalItems = locationsToAdd.Count;
            var reportPeriod = TimeSpan.FromSeconds(1);

            using (new Timer(_ => callback(processedItems / (totalItems/100)), null, reportPeriod, reportPeriod))
            {
                Parallel.ForEach(locationsToAdd, delegate(GeoLocationModel x, ParallelLoopState i, long l)
                {
                    GeoLocationRepository.Current.Add(x.PostalCode, x.City);
                    Interlocked.Increment(ref processedItems);
                });
            }
        }
    }
}