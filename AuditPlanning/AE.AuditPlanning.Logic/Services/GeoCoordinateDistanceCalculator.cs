using System;
using AE.AuditPlanning.Storage.Entities;

namespace AE.AuditPlanning.Logic.Services
{
    public static class GeoCoordinateDistanceCalculator
    {
        /// <summary>
        /// Equator perimeter in kilometers
        /// </summary>
        private const double EquatorPerimeter = 6378.137;

        /// <summary>
        /// Calculates the distance between the two given locations
        /// </summary>
        /// <returns>Distance in kilometers</returns>
        public static double GetDistance(GeoLocation origin, GeoLocation target)
        {
            var originLatitude = DegreeToRadian(origin.Latitude);
            var originLongitude = DegreeToRadian(origin.Longitude);
            var targetLatitude = DegreeToRadian(target.Latitude);
            var targetLongitude = DegreeToRadian(target.Longitude);

            var d = (Math.Sin(originLatitude) * Math.Sin(targetLatitude)) + (Math.Cos(originLatitude) * Math.Cos(targetLatitude) * Math.Cos(targetLongitude - originLongitude));
            d = d > 1 ? 1 : d;
            d = d < -1 ? -1 : d;
            var dist = Math.Acos(d);

            return dist * EquatorPerimeter;
        }

        /// <summary>
        /// Calculates the distance between the two given locations
        /// </summary>
        /// <returns>Distance in kilometers</returns>
        public static double DistanceTo(this GeoLocation origin, GeoLocation target)
        {
            var originLatitude = DegreeToRadian(origin.Latitude);
            var originLongitude = DegreeToRadian(origin.Longitude);
            var targetLatitude = DegreeToRadian(target.Latitude);
            var targetLongitude = DegreeToRadian(target.Longitude);

            var d = (Math.Sin(originLatitude) * Math.Sin(targetLatitude)) + (Math.Cos(originLatitude) * Math.Cos(targetLatitude) * Math.Cos(targetLongitude - originLongitude));
            d = d > 1 ? 1 : d;
            d = d < -1 ? -1 : d;
            var dist = Math.Acos(d);

            return dist * EquatorPerimeter;
        }

        private static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}