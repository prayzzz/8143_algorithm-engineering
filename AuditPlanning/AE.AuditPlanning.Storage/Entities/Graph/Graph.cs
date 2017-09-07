using System.Collections.Generic;

namespace AE.AuditPlanning.Storage.Entities.Graph
{
    public class Graph<T> : Entity, IGraph<T>
    {
        public Graph()
        {
            this.Nodes = new List<Node<T>>();
        }

        public List<Node<T>> Nodes { get; set; }

        public Node<T> StartNode { get; set; }
    }
}