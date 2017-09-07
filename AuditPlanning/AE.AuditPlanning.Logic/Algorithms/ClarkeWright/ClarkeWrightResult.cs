using System.Collections.Generic;

namespace AE.AuditPlanning.Logic.Algorithms.ClarkeWright
{
    public class ClarkeWrightResult<TS, TN>
    {
        public ClarkeWrightResult()
        {
            this.Routes = new List<List<TN>>();
        }

        public List<List<TN>> Routes { get; set; }

        public TS StartLocation { get; set; }
    }
}