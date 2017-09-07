using System;
using System.Collections.Generic;
using System.Linq;
using AE.AuditPlanning.Storage.Entities.Graph;

namespace AE.AuditPlanning.Logic.Algorithms.TSP
{
    public class NearestNeighbourConstraint : NearestNeighbourConstraintBase, INearestNeighbourConstraint
    {
        public NearestNeighbourConstraint(int nodesPerRoute, double distancePerRoute)
            :base(nodesPerRoute, distancePerRoute)
        {
        }

        public NearestNeighbourConstraint(double distancePerRoute)
            :base(distancePerRoute)
        {
        }

        public NearestNeighbourConstraint(int nodesPerRoute)
            : base(nodesPerRoute)
        {
        }

        public NearestNeighbourConstraint()
        {
        }

        public bool IsAddAllowed(ArrayGraph graph, IReadOnlyList<int> route, int nextNode)
        {
            if (route.Count + 1 > this.NodesPerRoute)
            {
                return false;
            }

            if (GetDistanceAfterAdd(graph, route, nextNode) > this.DistancePerRoute)
            {
                return false;
            }

            return true;
        }

        private static double GetDistanceAfterAdd(ArrayGraph graph, IReadOnlyList<int> route, int nextNode)
        {
            if (!route.Any())
            {
                return double.MinValue;
            }

            var distance = graph[route[0]];
            for (var i = 0; i < route.Count - 1; i++)
            {
                distance += graph[route[i], route[i + 1]];
            }

            distance += graph[route[route.Count - 1], nextNode];
            distance += graph[nextNode];
            return distance;
        }
    }
}