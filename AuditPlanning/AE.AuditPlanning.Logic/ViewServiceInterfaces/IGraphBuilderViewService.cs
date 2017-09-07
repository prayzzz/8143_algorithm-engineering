using System.Collections.Generic;

using AE.AuditPlanning.Logic.Models;

namespace AE.AuditPlanning.Logic.ViewServiceInterfaces
{
    public interface IGraphBuilderViewService
    {
        double CalculateRouteWithNearestNeighbour();

        List<EdgeModel> CalculateRouteWithClarkWright();

        int BuildGraph(int? graphId, string filePath, string startAdress);

        IEnumerable<NodeModel> GetNodes(int? graphId, int nodeSize);
    }
}