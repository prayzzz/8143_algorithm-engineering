using System.Collections.Generic;
using AE.AuditPlanning.Storage.Entities.Graph;

namespace AE.AuditPlanning.Logic.Algorithms.TSP
{
    public interface INearestNeighbourConstraint
    {
        bool IsAddAllowed(ArrayGraph graph, IReadOnlyList<int> currentRoute, int nextNode);
    }
}