using System.Diagnostics;

using AE.AuditPlanning.Logic.Base;

namespace AE.AuditPlanning.Logic.Models
{
    [DebuggerDisplay("Customer at {DisplayString}")]
    public class CustomerModel : BaseModel
    {

        public string PostalCode { get; set; }

        public string City { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int Items { get; set; }

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