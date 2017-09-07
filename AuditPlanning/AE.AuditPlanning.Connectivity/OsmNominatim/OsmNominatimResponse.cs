﻿namespace AE.AuditPlanning.Connectivity.OsmNominatim
{
    public class OsmPlace
    {
        public string place_id { get; set; }

        public string licence { get; set; }

        public string osm_type { get; set; }

        public string osm_id { get; set; }

        public string[] boundingbox { get; set; }

        public double lat { get; set; }

        public double lon { get; set; }

        public string display_name { get; set; }

        public string _class { get; set; }

        public string type { get; set; }

        public float importance { get; set; }

        public string icon { get; set; }
    }

}