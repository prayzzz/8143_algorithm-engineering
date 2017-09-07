using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AE.AuditPlanning.Common.Logging;
using AE.AuditPlanning.Connectivity.OsmNominatim;
using AE.AuditPlanning.Storage.Entities;
using AE.AuditPlanning.Storage.Loader;

namespace AE.AuditPlanning.Storage.Repositories
{
    public class GeoLocationRepository
    {
        private const string LocationFile = "locations.json";

        private readonly Dictionary<int, GeoLocation> geoLocations;

        private static GeoLocationRepository instance;

        private int currentId;

        private GeoLocationRepository()
        {
            this.currentId = 1;
            this.geoLocations = new Dictionary<int, GeoLocation>();

            Logger.LogDebug("GeoLocationRepository", ">> Loading Locations");
            var time = CallWithStopwatch(this.LoadGeoLocations);
            Logger.LogDebug("GeoLocationRepository", "<< Loading Locations in {0}ms", time);
        }

        public static GeoLocationRepository Current
        {
            get
            {
                return instance ?? (instance = new GeoLocationRepository());
            }
        }

        public GeoLocation Add(int postalCode, string city)
        {
            GeoLocation location;
            if (this.geoLocations.TryGetValue(postalCode, out location))
            {
                return location;
            }

            location = GetGetLocation(postalCode, city);
            if (location != null && this.SaveToRepository(location))
            {
                return location;
            }

            return null;
        }

        public GeoLocation Get(int postalCode)
        {
            GeoLocation location;
            if (this.geoLocations.TryGetValue(postalCode, out location))
            {
                return location;
            }

            return null;
        }

        public IEnumerable<GeoLocation> GetList()
        {
            return this.geoLocations.Select(x => x.Value);
        }

        public void Delete(GeoLocation location)
        {
            this.geoLocations.Remove(location.PostalCode);
        }

        private bool SaveToRepository(GeoLocation location)
        {
            location.Id = this.currentId;
            this.geoLocations.Add(location.PostalCode, location);
            this.currentId++;

            return true;
        }

        public void Shutdown()
        {
            JsonHelper.Save(LocationFile, this.geoLocations.Select(x => x.Value));
        }

        private static GeoLocation GetGetLocation(int postalCode, string city)
        {
            var coordinates = OsmNominatimServiceGermany.GetGeoCoordinates(postalCode.ToString("D5"), city);

            if (coordinates.IsValid)
            {
                return new GeoLocationImpl { PostalCode = postalCode, City = city, Latitude = coordinates.Latitude, Longitude = coordinates.Longitude };
            }

            return null;
        }

        private static long CallWithStopwatch(Action action)
        {
            var watch = new Stopwatch();
            watch.Start();

            action.Invoke();

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        private void LoadGeoLocations()
        {
            foreach (var location in JsonHelper.LoadList<GeoLocationImpl>(LocationFile))
            {
                this.SaveToRepository(location);
            }
        }

        private class GeoLocationImpl : GeoLocation
        {
        }
    }
}
