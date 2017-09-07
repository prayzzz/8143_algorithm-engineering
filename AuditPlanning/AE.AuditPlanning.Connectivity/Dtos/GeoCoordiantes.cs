namespace AE.AuditPlanning.Connectivity.Dtos
{
    public struct GeoCoordiantes
    {
        internal GeoCoordiantes(double latitude, double longitude)
            : this()
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.IsValid = true;
        }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public bool IsValid { get; private set; }

        public static GeoCoordiantes Invalid
        {
            get
            {
                return new GeoCoordiantes { IsValid = false };
            }
        }
    }
}