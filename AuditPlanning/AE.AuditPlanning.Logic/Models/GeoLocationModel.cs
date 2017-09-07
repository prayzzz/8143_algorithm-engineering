namespace AE.AuditPlanning.Logic.Models
{
    public class GeoLocationModel
    {
        public int PostalCode { get; set; }

        public string PostalCodeDisplay
        {
            get
            {
                return this.PostalCode.ToString("D5");
            }
        }

        public string City { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsEdited { get; set; }
    }
}