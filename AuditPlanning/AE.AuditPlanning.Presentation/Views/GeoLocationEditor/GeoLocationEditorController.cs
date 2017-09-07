using System.Threading.Tasks;
using System.Windows;
using AE.AuditPlanning.Logic;
using AE.AuditPlanning.Logic.Models;
using AE.AuditPlanning.Logic.ViewServiceInterfaces;
using AE.AuditPlanning.Presentation.Base;
using AE.AuditPlanning.Presentation.Common;

namespace AE.AuditPlanning.Presentation.Views.GeoLocationEditor
{
    public class GeoLocationEditorController : ViewControllerBase<GeoLocationEditorModel>
    {
        private readonly IGeoLocationEditorViewService service;

        public GeoLocationEditorController()
        {
            this.service = ServiceLocator.GeoLocationEditor;

            this.DeleteCommand = new RelayCommand<GeoLocationModel>(this.DeleteGeoLocation);
            this.LoadCommand = new RelayCommand(this.LoadGeoLocations, this.CanLoadExecute);

            this.Model.SeparatorSign = ";";
            this.GetGeoLocations();
        }

        private bool CanLoadExecute()
        {
            return !string.IsNullOrEmpty(this.Model.SeparatorSign);
        }

        private void LoadGeoLocations()
        {
            var path = FileDialogHelper.OpenCsvFileDialog("");

            this.Model.IsWorking = true;
            Task.Run(() => this.service.LoadNewGeoLocationsFromFile(path, this.Model.SeparatorSign, this.ProgressCallback))
                .ContinueWith(t => Application.Current.Dispatcher.Invoke(() =>
                {
                    this.GetGeoLocations();
                    this.Model.IsWorking = false;
                }));
        }

        public RelayCommand LoadCommand { get; set; }

        public RelayCommand<GeoLocationModel> DeleteCommand { get; set; }

        private void DeleteGeoLocation(GeoLocationModel model)
        {
            if (this.service.Delete(model))
            {
                this.Model.GeoLocations.Remove(model);
                this.Model.SelectedGeoLocation = null;
            }
        }

        private void GetGeoLocations()
        {
            this.Model.GeoLocations.Reset(this.service.GetGeoLocations());
        }

        private void ProgressCallback(int percentage)
        {
            this.Model.AddToDbProgress = percentage;
        }
    }
}