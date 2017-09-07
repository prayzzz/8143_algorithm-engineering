using System.Collections.ObjectModel;
using AE.AuditPlanning.Logic.Models;
using AE.AuditPlanning.Presentation.Base;

namespace AE.AuditPlanning.Presentation.Views.GraphBuilder
{
    public class GraphBuilderViewModel : ViewModelBase
    {
        private string filePath;
        private double nearestNeighbour;
        private double clarkWright;

        public GraphBuilderViewModel()
        {
            this.Nodes = new ObservableCollection<NodeModel>();
            this.Edges = new ObservableCollection<EdgeModel>();
        }

        public string FilePath
        {
            get
            {
                return this.filePath;
            }

            set
            {
                if (value == this.filePath)
                {
                    return;
                }

                this.filePath = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<NodeModel> Nodes { get; private set; }

        public ObservableCollection<EdgeModel> Edges { get; private set; }

        public string StartLocation { get; set; }

        public int NodeSize { get; set; }

        public double NearestNeighbour
        {
            get
            {
                return this.nearestNeighbour;
            }

            set
            {
                if (value == this.nearestNeighbour)
                {
                    return;
                }

                this.nearestNeighbour = value;
                this.OnPropertyChanged();
            }
        }

        public double ClarkWright
        {
            get
            {
                return this.clarkWright;
            }

            set
            {
                if (value == this.clarkWright)
                {
                    return;
                }

                this.clarkWright = value;
                this.OnPropertyChanged();
            }
        }

        public int? GraphId { get; set; }
    }
}