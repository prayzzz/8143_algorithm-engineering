using System;
using AE.AuditPlanning.Logic;
using AE.AuditPlanning.Logic.ViewServiceInterfaces;
using AE.AuditPlanning.Presentation.Base;
using AE.AuditPlanning.Presentation.Common;

namespace AE.AuditPlanning.Presentation.Views.GraphBuilder
{
    public class GraphBuilderViewController : ViewControllerBase<GraphBuilderViewModel>
    {
        private readonly IGraphBuilderViewService service;

        public GraphBuilderViewController()
        {
            this.service = ServiceLocator.GraphBuilder;

            this.BrowseCommand = new RelayCommand(this.Browse);
            this.BuildGraphCommand = new RelayCommand(this.BuildGraph);
            this.NearestNeighbourCommand = new RelayCommand(this.NearestNeighbour);
            this.ClarkWrightCommand = new RelayCommand(this.ClarkWright);

            this.Model.NodeSize = 2;
            this.Model.StartLocation = "...";
        }

        public RelayCommand BrowseCommand { get; private set; }

        public RelayCommand BuildGraphCommand { get; private set; }

        public RelayCommand NearestNeighbourCommand { get; private set; }

        public RelayCommand ClarkWrightCommand { get; private set; }

        private void Browse()
        {
            this.Model.FilePath = FileDialogHelper.OpenFileDialog(this.Model.FilePath, ".json", "Json Datei (*.json)|*.json");
        }

        private void NearestNeighbour()
        {
            this.Model.NearestNeighbour = Math.Round(this.service.CalculateRouteWithNearestNeighbour(), 2);
        }

        private void ClarkWright()
        {
            this.Model.Edges.Clear();
            foreach (var edge in this.service.CalculateRouteWithClarkWright())
            {
                this.Model.Edges.Add(edge);
            }
        }

        private void BuildGraph()
        {
            this.Model.GraphId = this.service.BuildGraph(this.Model.GraphId, this.Model.FilePath, this.Model.StartLocation);

            this.LoadNodes();
        }

        private void LoadNodes()
        {
            this.Model.Nodes.Clear();
            this.Model.Edges.Clear();

            foreach (var node in this.service.GetNodes(this.Model.GraphId, this.Model.NodeSize))
            {
                this.Model.Nodes.Add(node);
            }
        }
    }
}