namespace AE.AuditPlanning.Logic.Algorithms.ClarkeWrightConstraints
{
    public abstract class ClarkeWrightConstraintBase
    {
        protected ClarkeWrightConstraintBase(int nodesPerRoute, double distancePerRoute)
        {
            this.NodesPerRoute = nodesPerRoute;
            this.DistancePerRoute = distancePerRoute;
        }

        protected ClarkeWrightConstraintBase(double distancePerRoute)
        {
            this.NodesPerRoute = int.MaxValue;
            this.DistancePerRoute = distancePerRoute;
        }

        protected ClarkeWrightConstraintBase(int nodesPerRoute)
        {
            this.NodesPerRoute = nodesPerRoute;
            this.DistancePerRoute = double.MaxValue;
        }

        protected ClarkeWrightConstraintBase()
        {
            this.NodesPerRoute = int.MaxValue;
            this.DistancePerRoute = double.MaxValue;
        }

        public int NodesPerRoute { get; private set; }

        public double DistancePerRoute { get; private set; }
    }
}