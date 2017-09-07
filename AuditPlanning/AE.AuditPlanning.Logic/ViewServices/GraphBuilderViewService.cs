using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AE.AuditPlanning.Common.Logging;
using AE.AuditPlanning.Logic.Algorithms.TSP;
using AE.AuditPlanning.Logic.Models;
using AE.AuditPlanning.Logic.ViewServiceInterfaces;
using AE.AuditPlanning.Storage;
using AE.AuditPlanning.Storage.Converter;
using AE.AuditPlanning.Storage.Entities;
using AE.AuditPlanning.Storage.Entities.Graph;
using AE.AuditPlanning.Storage.Loader;
using AE.AuditPlanning.Storage.Repositories;

namespace AE.AuditPlanning.Logic.ViewServices
{
    public class GraphBuilderViewService : IGraphBuilderViewService
    {
        public int BuildGraph(int? graphId, string filePath, string startAdress)
        {
            if (graphId > 0)
            {
                Repository.Current.Delete<ArrayGraph>(graphId);
            }

            JsonHelper.LoadListInRepository<Customer>(filePath);

            ////var locations = Repository.Current.GetList<Customer>().Select(x => GeoLocationRepository.Current.Get(x.PostalCode));
            ////var graph = GeoLocationGraphBuilder.BuildGraphArray(locations, startAdress);

            return 0;
        }

        public IEnumerable<NodeModel> GetNodes(int? graphId, int nodeSize)
        {
            if (graphId < 1)
            {
                return new List<NodeModel>();
            }

            var graph = Repository.Current.GetById<Graph<GeoLocation>>(graphId);

            if (graph == null)
            {
                return new List<NodeModel>();
            }

            return graph.Nodes.Select(x => EntityToModel(x, nodeSize)).ToList();
        }

        public double CalculateRouteWithNearestNeighbour()
        {
            var graph = Repository.Current.GetById<Graph<GeoLocation>>(1);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            ////var route = new NearestNeighbourArray(new NearestNeighbourConstraint()).CalculateRoute(graph);
            stopWatch.Stop();

            Logger.LogDebug("ViewService", "Calculated route with Nearest-Neighbour Algorithm ({0}ms)", stopWatch.ElapsedMilliseconds);

            return 1; // route.Sum(x => x.Distance);
        }

        public List<EdgeModel> CalculateRouteWithClarkWright()
        {
            var graph = Repository.Current.GetById<Graph<GeoLocation>>(1);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            // var route = new ClarkeWrightHub<GeoLocation, Customer>(new GeoLocationToCustomerConstraint(), new GeoLocationToCustomerSavingsCalculator()).CalculateRoute(graph);
            stopWatch.Stop();

            Logger.LogDebug("ViewService", "Calculated route with Clark - Wright Algorithm ({0}ms)", stopWatch.ElapsedMilliseconds);

            return new List<EdgeModel>(); //route.SelectMany(x => x).Select(EntityToModel).ToList());
        }

        private static EdgeModel EntityToModel(Edge<GeoLocation> edge)
        {
            var edgeModel = new EdgeModel();
            edgeModel.StartX = CalculateCoordinate(edge.FromNode.Data.Longitude);
            edgeModel.StartY = -CalculateCoordinate(edge.FromNode.Data.Latitude);

            edgeModel.EndX = CalculateCoordinate(edge.ToNode.Data.Longitude);
            edgeModel.EndY = -CalculateCoordinate(edge.ToNode.Data.Latitude);

            return edgeModel;
        }

        private static NodeModel EntityToModel(Node<GeoLocation> node, int nodeSize)
        {
            var nodeModel = new NodeModel();
            nodeModel.X = CalculateCoordinate(node.Data.Longitude) - (nodeSize / 2);
            nodeModel.Y = CalculateCoordinate(node.Data.Latitude) - (nodeSize / 2);
            nodeModel.NodeSize = nodeSize;

            return nodeModel;
        }

        private static double CalculateCoordinate(double value)
        {
            return value * 50;
        }
    }
}