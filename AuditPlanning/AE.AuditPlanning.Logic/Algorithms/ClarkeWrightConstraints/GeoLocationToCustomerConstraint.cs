using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AE.AuditPlanning.Logic.Services;
using AE.AuditPlanning.Storage.Entities;
using AE.AuditPlanning.Storage.Entities.HubGraph;

namespace AE.AuditPlanning.Logic.Algorithms.ClarkeWrightConstraints
{
    public class GeoLocationToCustomerConstraint : ClarkeWrightConstraintBase, IClarkeWrightHubConstraint<GeoLocation, Customer>
    {
        public GeoLocationToCustomerConstraint(int nodesPerRoute, double distancePerRoute)
            :base(nodesPerRoute, distancePerRoute)
        {
        }

        public GeoLocationToCustomerConstraint(double distancePerRoute)
            :base(distancePerRoute)
        {
        }

        public GeoLocationToCustomerConstraint(int nodesPerRoute)
            : base(nodesPerRoute)
        {
        }

        public GeoLocationToCustomerConstraint()
        {
        }

        public bool IsMergeAllowed(HubGraph<GeoLocation, Customer> graph, IReadOnlyList<Customer> frontRoute, IReadOnlyList<Customer> rearRoute)
        {
            if (frontRoute.Count + rearRoute.Count > this.NodesPerRoute)
            {
                return false;
            }

            var distanceAfterMerge = DistanceAfterMerge(graph.Hub, frontRoute.Select(x => x.Location), rearRoute.Select(x => x.Location));
            if (distanceAfterMerge > this.DistancePerRoute)
            {
                return false;
            }

            return true;
        }

        private static double DistanceAfterMerge(GeoLocation hub, IEnumerable<GeoLocation> route, IEnumerable<GeoLocation> mergeRoute)
        {
            var routeOne = route as GeoLocation[] ?? route.ToArray();
            var routeTwo = mergeRoute as GeoLocation[] ?? mergeRoute.ToArray();

            if (!routeOne.Any() || !routeTwo.Any())
            {
                return double.MinValue;
            }

            var distance = hub.DistanceTo(routeOne.First());

            for (var i = 0; i < routeOne.Length - 1; i++)
            {
                distance += routeOne[i].DistanceTo(routeOne[i + 1]);
            }
            
            distance += routeOne.Last().DistanceTo(routeTwo.First());
            for (var i = 0; i < routeTwo.Length - 1; i++)
            {
                distance += routeTwo[i].DistanceTo(routeTwo[i + 1]);
            }

            distance += routeTwo.Last().DistanceTo(hub);
            return distance;
        }
    }
}