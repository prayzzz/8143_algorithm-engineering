using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using AE.AuditPlanning.Logic.Algorithms.ClarkeWright;
using AE.AuditPlanning.Logic.Algorithms.ClarkeWrightConstraints;
using AE.AuditPlanning.Logic.Algorithms.ClarkeWrightSavings;
using AE.AuditPlanning.Logic.Algorithms.TSP;
using AE.AuditPlanning.Logic.Services;
using AE.AuditPlanning.Storage;
using AE.AuditPlanning.Storage.Entities;
using AE.AuditPlanning.Storage.Entities.Graph;
using AE.AuditPlanning.Storage.Loader;
using AE.AuditPlanning.Storage.Repositories;

namespace AE.AuditPlanning.Perfomance
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length % 2 == 1)
            {
                printUsage();
                return;
            }

            var fileName = string.Empty;
            var nodeCount = 10;
            var repeatCount = 1;
            var output = true;

            var routeLength = 1000D;
            var routeNodeCount = int.MaxValue;

            var argMapping = new Dictionary<string, Action<string>>();
            argMapping.Add("--f", s => fileName = s);
            argMapping.Add("--n", s => nodeCount = Convert.ToInt32(s));
            argMapping.Add("--r", s => repeatCount = Convert.ToInt32(s));
            argMapping.Add("--o", s => output = Convert.ToBoolean(s));
            argMapping.Add("--rl", s => routeLength = Convert.ToDouble(s));
            argMapping.Add("--rnc", s => routeNodeCount = Convert.ToInt32(s));

            for (var i = 0; i < args.Length; i = i + 2)
            {
                Action<string> mapping;
                if (argMapping.TryGetValue(args[i], out mapping))
                {
                    mapping.Invoke(args[i + 1]);
                }
            }

            Console.WriteLine("Arguments:");
            Console.WriteLine("\t fileName \t " + fileName);
            Console.WriteLine("\t nodeCount \t " + nodeCount);
            Console.WriteLine("\t repeatCount \t " + repeatCount);
            Console.WriteLine("\t output \t " + output);
            Console.WriteLine("\t routeLength \t " + routeLength);
            Console.WriteLine("\t routeNodeCount \t " + routeNodeCount);

            Console.WriteLine();
            Console.WriteLine("Loading customers...");

            foreach (var customer in LoadFromCsv(fileName).Where(x => x.Location != null))
            {
                Repository.Current.Add(customer);
            }

            var customers = Repository.Current.GetList<Customer>().Take(nodeCount).ToList();

            Console.WriteLine("Start Location: ...");
            Console.WriteLine("Building graph with {0} nodes...", customers.Count());

            Console.WriteLine();
            Console.WriteLine("### Clarke-Wright - ArrayGraph ###");
            for (var i = 0; i < repeatCount; i++)
            {
                ArrayGraph(customers, output, routeLength, routeNodeCount);
            }

            Console.WriteLine();
            Console.WriteLine("### Clarke-Wright - HubGraph ###");

            for (var i = 0; i < repeatCount; i++)
            {
                Hub(customers, output, routeLength, routeNodeCount);
            }

            Console.WriteLine();
            Console.WriteLine("### Nearest Neighbour - ArrayGraph ###");

            for (var i = 0; i < repeatCount; i++)
            {
                NearestNeighbour(customers, output, routeLength, routeNodeCount);
            }

            Console.ReadLine();
        }

        private static List<List<Edge<GeoLocation>>> NearestNeighbour(List<Customer> customers, bool outputRoutes, double routeLength, int routeNodeCount)
        {
            var graph = CustomerGraphBuilder.BuildGraphArray(customers, "...");
            var constraint = new NearestNeighbourConstraint(routeNodeCount, routeLength);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var routes = RouteToEdges(new NearestNeighbourArray(constraint).CalculateRoute(graph), graph.StartLocation);
            stopWatch.Stop();

            Console.WriteLine("{1}", routes.Count(), stopWatch.ElapsedMilliseconds);

            if (outputRoutes)
            {
                foreach (var route in routes)
                {
                    Console.WriteLine("Length: {0}km", route.Sum(x => x.Distance));
                    Console.WriteLine(route.First().FromNode.Data.City + " -> " + String.Join(" -> ", route.Select(x => x.ToNode.Data.City)));
                }
            }

            return routes;
        }

        private static IEnumerable<List<Edge<GeoLocation>>> Hub(List<Customer> customers, bool outputRoutes, double routeLength, int routeNodeCount)
        {
            var g = CustomerGraphBuilder.BuildHubGraph(customers, "...");
            var constraint = new GeoLocationToCustomerConstraint(routeNodeCount, routeLength);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var routes = RouteToEdges(new ClarkeWrightHub<GeoLocation, Customer>(constraint, new GeoLocationToCustomerSavingsCalculator()).CalculateRoute(g));
            stopWatch.Stop();

            Console.WriteLine("{1}", routes.Count(), stopWatch.ElapsedMilliseconds);

            if (outputRoutes)
            {
                foreach (var route in routes)
                {
                    Console.WriteLine("Length: {0}km", route.Sum(x => x.Distance));
                    Console.WriteLine(route.First().FromNode.Data.City + " -> " + String.Join(" -> ", route.Select(x => x.ToNode.Data.City)));
                }
            }

            return routes;
        }

        private static IEnumerable<List<Edge<GeoLocation>>> ArrayGraph(List<Customer> customers, bool outputRoutes, double routeLength, int routeNodeCount)
        {
            var graph = CustomerGraphBuilder.BuildGraphArray(customers, "...");
            var constraint = new ArrayConstraint(routeNodeCount, routeLength);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var routes = RouteToEdges(new ClarkeWrightArray(constraint).CalculateRoute(graph), graph.StartLocation);
            stopWatch.Stop();

            Console.WriteLine("{1}", routes.Count(), stopWatch.ElapsedMilliseconds);

            if (outputRoutes)
            {
                foreach (var route in routes)
                {
                    Console.WriteLine("Length: {0}km", route.Sum(x => x.Distance));
                    Console.WriteLine(route.First().FromNode.Data.City + " -> " + String.Join(" -> ", route.Select(x => x.ToNode.Data.City)));
                }
            }

            return routes;
        }

        private static List<List<Edge<GeoLocation>>> RouteToEdges(ClarkeWrightResult<GeoLocation, Customer> routes)
        {
            var list = new List<List<Edge<GeoLocation>>>();

            foreach (var route in routes.Routes)
            {
                var edges = new List<Edge<GeoLocation>>();
                list.Add(edges);

                //// edge from depot to first node
                var fromNode = new Node<GeoLocation>(routes.StartLocation);
                var toNode = new Node<GeoLocation>(route.First().Location);
                edges.Add(new Edge<GeoLocation>(fromNode.Data.DistanceTo(toNode.Data), fromNode, toNode));

                for (var i = 0; i < route.Count - 1; i++)
                {
                    fromNode = new Node<GeoLocation>(route[i].Location);
                    toNode = new Node<GeoLocation>(route[i + 1].Location);
                    edges.Add(new Edge<GeoLocation>(fromNode.Data.DistanceTo(toNode.Data), fromNode, toNode));
                }

                //// edge from last node to depot
                fromNode = new Node<GeoLocation>(route.Last().Location);
                toNode = new Node<GeoLocation>(routes.StartLocation);
                edges.Add(new Edge<GeoLocation>(fromNode.Data.DistanceTo(toNode.Data), fromNode, toNode));
            }

            return list;
        }

        private static List<List<Edge<GeoLocation>>> RouteToEdges(IEnumerable<List<int>> routes, GeoLocation startLocation)
        {
            var list = new List<List<Edge<GeoLocation>>>();

            foreach (var route in routes)
            {
                var edges = new List<Edge<GeoLocation>>();
                list.Add(edges);

                //// edge from depot to first node
                var fromNode = new Node<GeoLocation>(startLocation);
                var toNode = new Node<GeoLocation>(Repository.Current.GetById<Customer>(route[0] + 1).Location);
                edges.Add(new Edge<GeoLocation>(GeoCoordinateDistanceCalculator.GetDistance(fromNode.Data, toNode.Data), fromNode, toNode));

                //// edges between nodes
                for (var i = 0; i < route.Count - 1; i++)
                {
                    fromNode = new Node<GeoLocation>(Repository.Current.GetById<Customer>(route[i] + 1).Location);
                    toNode = new Node<GeoLocation>(Repository.Current.GetById<Customer>(route[i + 1] + 1).Location);
                    edges.Add(new Edge<GeoLocation>(GeoCoordinateDistanceCalculator.GetDistance(fromNode.Data, toNode.Data), fromNode, toNode));
                }

                //// edge from last node to depot
                fromNode = new Node<GeoLocation>(Repository.Current.GetById<Customer>(route[route.Count - 1] + 1).Location);
                toNode = new Node<GeoLocation>(startLocation);
                edges.Add(new Edge<GeoLocation>(GeoCoordinateDistanceCalculator.GetDistance(fromNode.Data, toNode.Data), fromNode, toNode));
            }

            return list;
        }

        private static IEnumerable<Customer> LoadFromCsv(string file)
        {
            Action<Customer, string[]> mapping = (c, s) =>
            {
                c.CustomerNumber = s[0];
                c.Location = GeoLocationRepository.Current.Add(int.Parse(s[1]), s[2]);
            };

            return CsvLoader.LoadCsv(file, ';', mapping);
        }

        private static void printUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("auditplanning.exe --f <FileName> --n <NodeCount> --r <Repetitions> --o <RouteOutput> --rl <MaxRouteLength> --rnc <MaxRouteNodeCount>");
            Console.WriteLine("\t --f \t String \t CSV-File Format: CustomerNumber;PostalCode;City");
            Console.WriteLine("\t --n \t Integer");
            Console.WriteLine("\t --r \t Integer");
            Console.WriteLine("\t --o \t Boolean");
            Console.WriteLine("\t --rl \t Double");
            Console.WriteLine("\t --rnc \t Integer");
        }
    }
}
