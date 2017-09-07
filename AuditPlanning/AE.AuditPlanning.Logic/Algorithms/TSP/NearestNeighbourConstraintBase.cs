namespace AE.AuditPlanning.Logic.Algorithms.TSP
{
    public abstract class NearestNeighbourConstraintBase
    {
        protected NearestNeighbourConstraintBase(int nodesPerRoute, double distancePerRoute)
        {
            this.NodesPerRoute = nodesPerRoute;
            this.DistancePerRoute = distancePerRoute;
        }

        protected NearestNeighbourConstraintBase(double distancePerRoute)
        {
            this.NodesPerRoute = int.MaxValue;
            this.DistancePerRoute = distancePerRoute;
        }

        protected NearestNeighbourConstraintBase(int nodesPerRoute)
        {
            this.NodesPerRoute = nodesPerRoute;
            this.DistancePerRoute = double.MaxValue;
        }

        protected NearestNeighbourConstraintBase()
        {
            this.NodesPerRoute = int.MaxValue;
            this.DistancePerRoute = double.MaxValue;
        }

        public int NodesPerRoute { get; private set; }

        public double DistancePerRoute { get; private set; }
    }
}