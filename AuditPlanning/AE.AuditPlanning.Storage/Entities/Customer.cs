using System;
using System.Diagnostics;

using Newtonsoft.Json;

namespace AE.AuditPlanning.Storage.Entities
{
    [Serializable]
    [DebuggerDisplay("Customer {DisplayString}")]
    public class Customer : Entity
    {
        public int Items { get; set; }

        public string CustomerNumber { get; set; }

        public int PostalCode { get; set; }

        public string City { get; set; }

        [JsonIgnore]
        public GeoLocation Location { get; set; }

        [JsonIgnore]
        public string DisplayString
        {
            get
            {
                return this.CustomerNumber;
            }
        }

        public override string ToString()
        {
            return this.DisplayString;
        }
    }
}