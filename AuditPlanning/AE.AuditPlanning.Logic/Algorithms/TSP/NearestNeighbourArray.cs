using System.Collections.Generic;
using System.Linq;
using AE.AuditPlanning.Common.Logging;
using AE.AuditPlanning.Storage.Entities.Graph;

namespace AE.AuditPlanning.Logic.Algorithms.TSP
{
    /// <summary>
    /// Berechnet die Route ueber das Verfahren des naechsten Nachbars
    /// </summary>
    public class NearestNeighbourArray
    {
        private readonly INearestNeighbourConstraint constraint;

        public NearestNeighbourArray(INearestNeighbourConstraint constraint)
        {
            this.constraint = constraint;
        }

        public List<List<int>> CalculateRoute(ArrayGraph graph)
        {
            var routes = new List<List<int>>();

            var availableNodes = Enumerable.Range(0, graph.Length).ToList();

            //// Startknoten
            var currentNode = availableNodes.First();
            availableNodes.Remove(currentNode);

            var currentRoute = new List<int> { currentNode };
            routes.Add(currentRoute);
            
            Logger.LogDebug("NearestNeighbour", "Creating Routes...");
            //// Solange noch unbetrachtete Knoten verfuegbar sind
            while (availableNodes.Any())
            {
                var distance = double.MaxValue;
                var nextNode = -1;

                foreach (var possibleNode in availableNodes)
                {
                    if (!(graph[currentNode, possibleNode] < distance))
                    {
                        continue;
                    }

                    nextNode = possibleNode;
                    distance = graph[currentNode, possibleNode];
                }

                if (!this.constraint.IsAddAllowed(graph, currentRoute, nextNode))
                {
                    //// Erstelle neue Route
                    currentRoute = new List<int> { nextNode };
                    routes.Add(currentRoute);
                }
                else
                {
                    //// Fuege Kante zur Route hinzu
                    currentRoute.Add(nextNode);
                }

                //// Bereite naechsten Durchlauf vor
                availableNodes.Remove(nextNode);
                currentNode = nextNode;
            }

            return routes;
        }
    }
}