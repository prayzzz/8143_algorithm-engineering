using System.Threading.Tasks;
using AE.AuditPlanning.Logic;
using AE.AuditPlanning.Logic.ViewServiceInterfaces;
using AE.AuditPlanning.Presentation.Base;
using AE.AuditPlanning.Presentation.Common;

namespace AE.AuditPlanning.Presentation.Views.JsonLoader
{
    public class JsonLoaderViewController : ViewControllerBase<JsonLoaderViewModel>
    {
        private readonly IJsonLoaderViewService service;

        public JsonLoaderViewController()
        {
            this.service = ServiceLocator.JsonLoader;

            this.BrowseCommand = new RelayCommand(this.Browse);
            this.LoadJsonCommand = new RelayCommand(this.LoadJson);
            this.LoadGeoDataCommand = new RelayCommand(this.LoadGeoData);
            this.SaveJsonCommand = new RelayCommand(this.SaveJson);
        }

        public RelayCommand BrowseCommand { get; set; }

        public RelayCommand LoadJsonCommand { get; set; }

        public RelayCommand LoadGeoDataCommand { get; set; }

        public RelayCommand SaveJsonCommand { get; set; }

        private void Browse()
        {
            this.Model.FilePath = FileDialogHelper.OpenFileDialog(this.Model.FilePath, ".json", "Kommaliste (*.json)|*.json");
        }

        private void SaveJson()
        {
            this.service.Save(this.Model.FilePath, this.Model.Customers);
        }

        private void LoadGeoData()
        {
            Task.Run(() => this.service.LoadGeoCordsParallel(this.Model.Customers, this.ProgressCallback));
        }

        private void LoadJson()
        {
            this.Model.Customers.Clear();
            foreach (var customer in this.service.LoadJson(this.Model.FilePath))
            {
                this.Model.Customers.Add(customer);
            }
        }

        private void ProgressCallback(int value)
        {
            if (value > 100 || value < 0)
            {
                return;
            }

            this.Model.TaskProgress = value;
        }
    }
}