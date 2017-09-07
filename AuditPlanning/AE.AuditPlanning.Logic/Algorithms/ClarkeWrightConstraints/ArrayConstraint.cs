using System.Collections.Generic;
using System.Linq;
using AE.AuditPlanning.Storage.Entities.Graph;

namespace AE.AuditPlanning.Logic.Algorithms.ClarkeWrightConstraints
{
    public class ArrayConstraint : ClarkeWrightConstraintBase, IClarkeWrightArrayConstraint
    {
        public ArrayConstraint(int nodesPerRoute, double distancePerRoute)
            :base(nodesPerRoute, distancePerRoute)
        {
        }

        public ArrayConstraint(double distancePerRoute)
            :base(distancePerRoute)
        {
        }

        public ArrayConstraint(int nodesPerRoute)
            : base(nodesPerRoute)
        {
        }

        public ArrayConstraint()
        {
        }

        public bool IsMergeAllowed(ArrayGraph graph, IReadOnlyList<int> frontRoute, IReadOnlyList<int> rearRoute)
        {
            if (frontRoute.Count + rearRoute.Count > this.NodesPerRoute)
            {
                return false;
            }

            if (GetDistanceAfterMerge(graph, frontRoute, rearRoute) > this.DistancePerRoute)
            {
                return false;
            }

            return true;
        }

        private static double GetDistanceAfterMerge(ArrayGraph graph, IReadOnlyList<int> route, IReadOnlyList<int> mergeRoute)
        {
            if (!route.Any() || !mergeRoute.Any())
            {
                return double.MinValue;
            }

            var distance = graph[route.First()];
            for (var i = 0; i < route.Count - 1; i++)
            {
                distance += graph[route[i], route[i + 1]];
            }

            distance += graph[route.Last(), mergeRoute.First()];
            for (var i = 0; i < mergeRoute.Count - 1; i++)
            {
                distance += graph[mergeRoute[i], mergeRoute[i + 1]];
            }

            distance += graph[mergeRoute.Last()];
            return distance;
        }
    }
}