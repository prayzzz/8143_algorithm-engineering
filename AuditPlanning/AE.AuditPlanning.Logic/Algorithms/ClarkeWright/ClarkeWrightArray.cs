using System.Collections.Generic;
using System.Linq;
using AE.AuditPlanning.Common.Logging;
using AE.AuditPlanning.Logic.Algorithms.ClarkeWrightConstraints;
using AE.AuditPlanning.Logic.Algorithms.ClarkeWrightSavings;
using AE.AuditPlanning.Storage.Entities.Graph;

namespace AE.AuditPlanning.Logic.Algorithms.ClarkeWright
{
    public class ClarkeWrightArray
    {
        private readonly IClarkeWrightArrayConstraint constraint;

        public ClarkeWrightArray(IClarkeWrightArrayConstraint constraint)
        {
            this.constraint = constraint;
        }

        /// <summary>
        /// Berechnet aus dem gegebenen Graph Routen nach dem Konzept des ClarkWright Algorithmus 
        /// </summary>
        /// <returns>Auflistung von Routen</returns>
        public IEnumerable<List<int>> CalculateRoute(ArrayGraph graph)
        {
            if (graph == null)
            {
                return new List<List<int>>();
            }

            //// Erstelle eine Route zu jedem Knoten. Es ist sichergestellt, dass jeder Knoten angefahren wird
            var routes = new Dictionary<int, List<int>>();
            for (var i = 0; i < graph.Length; i++)
            {
                routes.Add(i, new List<int> { i });
            }

            Logger.LogDebug("ClarkeWrightArray", "Calculating savings...");
            var savings = ComputeSavings(graph);

            Logger.LogDebug("ClarkeWrightArray", "Creating routes...");
            foreach (var saving in savings)
            {
                this.Merge(graph, routes, saving.FromNode, saving.ToNode);
            }

            Logger.LogDebug("ClarkeWrightArray", "Finished routes...");
            return routes.Select(x => x.Value);
        }

        private void Merge(ArrayGraph graph, Dictionary<int, List<int>> routes, int fromNode, int toNode)
        {
            if (this.InnerMerge(graph, routes, fromNode, toNode))
            {
                return;
            }

            if (this.InnerMerge(graph, routes, toNode, fromNode))
            {
            }
        }

        private bool InnerMerge(ArrayGraph graph, Dictionary<int, List<int>> routes, int fromNode, int toNode)
        {
            List<int> rearRoute;
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

        /// <summary>
        /// Berechnet die Einsparung für alle Knotenpaare
        /// Die Einsparung ist die Differenz aus dem Weg von einem Knoten über den Startknoten zum nächsten Knoten
        /// und dem direkten Weg zwischen beiden Knoten
        /// s(i, j) = d(D, i) + d(D, j) - d(i, j)
        /// </summary>
        /// <param name="graph"></param>
        /// <returns> List von zwei Knoten mit der Einsparung durch die Benutzung des direkten Weg </returns>
        private static IEnumerable<ClarkeWrightSaving<int>> ComputeSavings(ArrayGraph graph)
        {
            var savings = new List<ClarkeWrightSaving<int>>();

            for (var i = 0; i < graph.Length; i++)
            {
                for (var j = i + 1; j < graph.Length; j++)
                {
                    var saving = graph[i] + graph[j] - graph[i, j];
                    savings.Add(new ClarkeWrightSaving<int>(saving, i, j));
                }
            }

            return savings.OrderByDescending(x => x.Saving).ToList();
        }
    }
}