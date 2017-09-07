using System.Collections.Generic;
using System.Linq;
using AE.AuditPlanning.Logic.Services;
using AE.AuditPlanning.Storage.Entities;
using AE.AuditPlanning.Storage.Entities.HubGraph;

namespace AE.AuditPlanning.Logic.Algorithms.ClarkeWrightSavings
{
    public class GeoLocationToCustomerSavingsCalculator : IClarkeWrightHubSavingsCalculator<GeoLocation, Customer>
    {
        /// <summary>
        /// Berechnet die Einsparung für alle Knotenpaare
        /// Die Einsparung ist die Differenz aus dem Weg von einem Knoten über den Startknoten zum nächsten Knoten
        /// und dem direkten Weg zwischen beiden Knoten
        /// s(i, j) = d(D, i) + d(D, j) - d(i, j)
        /// </summary>
        public IEnumerable<ClarkeWrightSaving<Customer>> Calculate(HubGraph<GeoLocation, Customer> graph)
        {
            var savings = new List<ClarkeWrightSaving<Customer>>();

            var customers = graph.Nodes.ToList();

            foreach (var fromCustomer in graph.Nodes)
            {
                customers.Remove(fromCustomer);

                savings.AddRange(from toCustomer in customers
                                 let saving = fromCustomer.Value + toCustomer.Value - GeoCoordinateDistanceCalculator.GetDistance(fromCustomer.Key.Location, toCustomer.Key.Location)
                                 select new ClarkeWrightSaving<Customer>(saving, fromCustomer.Key, toCustomer.Key));
            }

            return savings;
        }
    }
}