using System;
using System.Collections.Generic;
using AE.AuditPlanning.Logic.Models;

namespace AE.AuditPlanning.Logic.ViewServiceInterfaces
{
    public interface IGeoLocationEditorViewService
    {
        IEnumerable<GeoLocationModel> GetGeoLocations();
        
        bool Delete(GeoLocationModel model);

        void LoadNewGeoLocationsFromFile(string path, string seperator);

        void LoadNewGeoLocationsFromFile(string path, string seperator, Action<int> callback);
    }
}