using System.Collections.Generic;
using AE.AuditPlanning.Storage.Entities.HubGraph;

namespace AE.AuditPlanning.Logic.Algorithms.ClarkeWrightSavings
{
    public interface IClarkeWrightHubSavingsCalculator<TS, TN>
    {
        IEnumerable<ClarkeWrightSaving<TN>> Calculate(HubGraph<TS, TN> graph);
    }
}