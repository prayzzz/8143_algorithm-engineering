namespace AE.AuditPlanning.Storage.Entities.Graph
{
    public class ArrayGraph : Entity
    {
        private readonly double[,] nodeDistances;

        private readonly double[] startDistances;

        public ArrayGraph(int length)
        {
            this.nodeDistances = new double[length, length];
            this.startDistances = new double[length];
        }

        public GeoLocation StartLocation { get; set; }

        /// <summary>
        /// Distance from node i to node j
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public double this[int i, int j]
        {
            get
            {
                return this.nodeDistances[i, j];
            }

            set
            {
                this.nodeDistances[i, j] = value;
            }
        }

        /// <summary>
        /// Distance from startLocation to node i
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double this[int i]
        {
            get
            {
                return this.startDistances[i];
            }

            set
            {
                this.startDistances[i] = value;
            }
        }

        public int Length
        {
            get
            {
                return this.startDistances.Length;
            }
        }
    }
}