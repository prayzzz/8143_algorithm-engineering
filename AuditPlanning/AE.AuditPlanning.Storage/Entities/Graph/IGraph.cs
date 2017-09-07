using System.Collections.Generic;

namespace AE.AuditPlanning.Storage.Entities.Graph
{
    public interface IGraph<T>
    {
        List<Node<T>> Nodes { get; set; }

        Node<T> StartNode { get; set; }
    }
}