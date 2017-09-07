using System;
using System.Diagnostics;

using Newtonsoft.Json;

namespace AE.AuditPlanning.Storage.Entities
{
    [Serializable]
    [DebuggerDisplay("Location {DisplayString}")]
    public abstract class GeoLocation : Entity
    {
        public int PostalCode { get; set; }

        public string City { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [JsonIgnore]
        public string DisplayString
        {
            get
            {
                return string.Format("{0}, {1}", this.PostalCode, this.City);
            }
        }

        public override string ToString()
        {
            return this.DisplayString;
        }
    }
}