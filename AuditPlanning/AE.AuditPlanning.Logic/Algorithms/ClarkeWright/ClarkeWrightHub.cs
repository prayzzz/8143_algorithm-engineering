using System.Collections.Generic;
using System.Linq;
using AE.AuditPlanning.Common.Logging;
using AE.AuditPlanning.Logic.Algorithms.ClarkeWrightConstraints;
using AE.AuditPlanning.Logic.Algorithms.ClarkeWrightSavings;
using AE.AuditPlanning.Storage.Entities.HubGraph;

namespace AE.AuditPlanning.Logic.Algorithms.ClarkeWright
{
    public class ClarkeWrightHub<TS, TN>
        where TN : class
        where TS : class
    {
        private readonly IClarkeWrightHubConstraint<TS, TN> constraint;

        private readonly IClarkeWrightHubSavingsCalculator<TS, TN> hubSavingsCalculator;

        public ClarkeWrightHub(IClarkeWrightHubConstraint<TS, TN> constraint, IClarkeWrightHubSavingsCalculator<TS, TN> hubSavingsCalculator)
        {
            this.constraint = constraint;
            this.hubSavingsCalculator = hubSavingsCalculator;
        }

        public ClarkeWrightResult<TS, TN> CalculateRoute(HubGraph<TS, TN> graph)
        {
            if (graph.Hub == null)
            {
                return new ClarkeWrightResult<TS, TN>();
            }

            var routes = graph.Nodes.ToDictionary(node => node.Key, node => new List<TN> { node.Key });

            Logger.LogDebug("ClarkeWrightHub", "Calculating savings...");
            var savings = this.hubSavingsCalculator.Calculate(graph).OrderByDescending(x => x.Saving);

            Logger.LogDebug("ClarkeWrightHub", "Creating routes...");
            foreach (var saving in savings)
            {
                this.Merge(graph, routes, saving.FromNode, saving.ToNode);
            }

            Logger.LogDebug("ClarkeWrightHub", "Finalizing...");
            var result = new ClarkeWrightResult<TS, TN>();
            result.StartLocation = graph.Hub;
            result.Routes.AddRange(routes.Select(x => x.Value));

            return result;
        }

        private void Merge(HubGraph<TS, TN> graph, IDictionary<TN, List<TN>> routes, TN fromNode, TN toNode)
        {
            if (this.InnerMerge(graph, routes, fromNode, toNode))
            {
                return;
            }

            this.InnerMerge(graph, routes, toNode, fromNode);
        }

        private bool InnerMerge(HubGraph<TS, TN> graph, IDictionary<TN, List<TN>> routes, TN fromNode, TN toNode)
        {
            List<TN> rearRoute;
            if (!routes.TryGetValue(fromNode, out rearRoute))
            {
                return false;
            }

            var frontRoute = routes.Values.FirstOrDefault(x => x.Last() == toNode);
            if (frontRoute == null || frontRoute == rearRoute)
            {
                return false;
            }

            if (!this.constraint.IsMergeAllowed(graph, frontRoute, rearRoute))
            {
                return false;
            }

            frontRoute.AddRange(rearRoute);
            routes.Remove(fromNode);

            return true;
        }
    }
}