using System.Collections.Generic;
using AE.AuditPlanning.Storage.Entities.HubGraph;

namespace AE.AuditPlanning.Logic.Algorithms.ClarkeWrightConstraints
{
    public interface IClarkeWrightHubConstraint<TS, TN>
    {
        bool IsMergeAllowed(HubGraph<TS, TN> graph, IReadOnlyList<TN> frontRoute, IReadOnlyList<TN> rearRoute);
    }
}