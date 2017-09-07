using System.Collections.Generic;
using System.Linq;
using System.Net;

using AE.AuditPlanning.Common.Logging;
using AE.AuditPlanning.Connectivity.Dtos;

using Newtonsoft.Json;

namespace AE.AuditPlanning.Connectivity.OsmNominatim
{
    public static class OsmNominatimServiceGermany
    {
        private const string GeoCodingUrl = "http://nominatim.openstreetmap.org/search?q={0}&format=json";

        private static GeoCoordiantes germanySouthWest = new GeoCoordiantes(47.2703, 5.8667);

        private static GeoCoordiantes germanyNorthEast = new GeoCoordiantes(55.0585, 15.0419);

        public static GeoCoordiantes GetGeoCoordinates(string postalCode, string city)
        {
            var requestUrl = string.Format(GeoCodingUrl, string.Format("{0}+{1}+Deutschland", postalCode, city.Replace(' ', '+')));
            var coordinates = TrySetGeoCords(requestUrl);
            if (CheckGermanBoundaries(coordinates))
            {
                return coordinates;
            }

            //// Try postal code only
            requestUrl = string.Format(GeoCodingUrl, string.Format("{0}+Deutschland", postalCode));
            coordinates = TrySetGeoCords(requestUrl);
            if (CheckGermanBoundaries(coordinates))
            {
                return coordinates;
            }

            Logger.LogDebug("OSM", "Could not load GeoCords for {0}, {1}", postalCode, city);
            return GeoCoordiantes.Invalid;
        }

        private static GeoCoordiantes TrySetGeoCords(string requestUrl)
        {
            var place = GetPlaceFromUrl(requestUrl);

            return place == null ? GeoCoordiantes.Invalid : new GeoCoordiantes(place.lat, place.lon);
        }

        private static OsmPlace GetPlaceFromUrl(string url)
        {
            Logger.LogDebug("OsmNominatimServiceGermany", "Requesting coordinates for " + url);
            using (var client = new WebClient())
            {
                var jsonData = client.DownloadString(url);
                var places = JsonConvert.DeserializeObject<IEnumerable<OsmPlace>>(jsonData);

                return places.OrderByDescending(x => x.importance)
                             .FirstOrDefault(x => x.type.ToLower() == "postcode" ||
                    x.type.ToLower() == "postal_code" ||
                    x.type.ToLower() == "village" ||
                    x.type.ToLower() == "hamlet" ||
                    x.type.ToLower() == "residential" ||
                    x.type.ToLower() == "allotments" ||
                    x.type.ToLower() == "suburb" ||
                    x.type.ToLower() == "town");
            }
        }

        private static bool CheckGermanBoundaries(GeoCoordiantes coordiantes)
        {
            if (coordiantes.Latitude < germanySouthWest.Latitude || coordiantes.Longitude < germanySouthWest.Longitude)
            {
                return false;
            }

            if (coordiantes.Latitude > germanyNorthEast.Latitude || coordiantes.Longitude > germanyNorthEast.Longitude)
            {
                return false;
            }

            return true;
        }
    }
}
