using System.Diagnostics;

namespace AE.AuditPlanning.Storage.Entities.Graph
{
    [DebuggerDisplay("Edge from {FromNode} to {ToNode} ({Distance})")]
    public class Edge<T>
    {
        public Edge(double distance, Node<T> fromNode, Node<T> toNode)
        {
            this.Distance = distance;
            this.FromNode = fromNode;
            this.ToNode = toNode;
        }

        public double Distance { get; private set; }

        public Node<T> FromNode { get; private set; }

        public Node<T> ToNode { get; private set; }

        public Edge<T> Reverse()
        {
            return new Edge<T>(this.Distance, this.ToNode, this.FromNode);
        }
    }
}