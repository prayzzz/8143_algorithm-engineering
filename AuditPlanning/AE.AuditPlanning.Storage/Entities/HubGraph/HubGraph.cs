using System.Collections.Generic;

namespace AE.AuditPlanning.Storage.Entities.HubGraph
{
    public class HubGraph<TS, TN>
    {
        public HubGraph()
        {
            this.Nodes = new Dictionary<TN, double>();
        }

        /// <summary>
        /// Node and his distance to the hub
        /// </summary>
        public Dictionary<TN, double> Nodes { get; set; }

        public TS Hub { get; set; }
    }
}