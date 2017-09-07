using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using AE.AuditPlanning.Storage.Entities.Graph.Exceptions;

namespace AE.AuditPlanning.Storage.Entities.Graph
{
    [DebuggerDisplay("Node for {Data}")]
    public class Node<T>
    {
        private readonly LinkedList<Edge<T>> edges;

        public Node(T data)
        {
            this.edges = new LinkedList<Edge<T>>();
            this.Data = data;
        }

        public IEnumerable<Edge<T>> Edges
        {
            get
            {
                return this.edges;
            }
        }

        public T Data { get; private set; }

        public void AddEdge(Edge<T> edge)
        {
            this.edges.AddLast(edge);
        }

        public Edge<T> EdgeTo(Node<T> node)
        {
            var edge = this.edges.FirstOrDefault(x => x.ToNode == node);

            if (edge == null)
            {
                throw new EdgeException(string.Format("No edge found from {0} to {1}", this, node));
            }

            return edge;
        }

        public override string ToString()
        {
            return string.Format("Node for " + this.Data);
        }
    }
}