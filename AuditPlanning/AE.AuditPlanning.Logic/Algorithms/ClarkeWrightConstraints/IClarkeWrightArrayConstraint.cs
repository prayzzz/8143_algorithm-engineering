using System.Collections.Generic;
using AE.AuditPlanning.Storage.Entities.Graph;

namespace AE.AuditPlanning.Logic.Algorithms.ClarkeWrightConstraints
{
    public interface IClarkeWrightArrayConstraint
    {
        bool IsMergeAllowed(ArrayGraph graph, IReadOnlyList<int> frontRoute, IReadOnlyList<int> rearRoute);
    }
}