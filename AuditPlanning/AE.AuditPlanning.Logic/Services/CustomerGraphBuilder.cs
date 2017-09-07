using System.Collections.Generic;
using System.Linq;
using AE.AuditPlanning.Storage;
using AE.AuditPlanning.Storage.Entities;
using AE.AuditPlanning.Storage.Entities.Graph;
using AE.AuditPlanning.Storage.Entities.HubGraph;
using AE.AuditPlanning.Storage.Repositories;

namespace AE.AuditPlanning.Logic.Services
{
    public static class CustomerGraphBuilder
    {
        public static HubGraph<GeoLocation, Customer> BuildHubGraph(IEnumerable<Customer> customers, string startAdress)
        {
            var graph = new HubGraph<GeoLocation, Customer>();
            graph.Hub = GetGeoLocation(startAdress);

            foreach (var customer in customers)
            {
                graph.Nodes.Add(customer, GeoCoordinateDistanceCalculator.GetDistance(customer.Location, graph.Hub));
            }

            return graph;
        }

        public static ArrayGraph BuildGraphArray(IEnumerable<Customer> customers, string startAdress)
        {
            var nodes = customers as IList<Customer> ?? customers.Where(x => x.IsStored).ToList();

            var graph = new ArrayGraph(nodes.Count());
            graph.StartLocation = GetGeoLocation(startAdress);

            //// Distances between Nodes
            foreach (var fromNode in nodes)
            {
                foreach (var toNode in nodes)
                {
                    graph[fromNode.Id.Value - 1, toNode.Id.Value - 1] = GeoCoordinateDistanceCalculator.GetDistance(fromNode.Location, toNode.Location);
                }
            }

            //// Distances between StartLocation and Nodes
            foreach (var toNode in nodes)
            {
                graph[toNode.Id.Value - 1] = GeoCoordinateDistanceCalculator.GetDistance(graph.StartLocation, toNode.Location);
            }

            return graph;
        }

        private static GeoLocation GetGeoLocation(string startAdress)
        {
            var split = startAdress.Split(',');

            var postalCode = int.Parse(split[0].Trim());

            return GeoLocationRepository.Current.Get(postalCode);
        }
    }
}