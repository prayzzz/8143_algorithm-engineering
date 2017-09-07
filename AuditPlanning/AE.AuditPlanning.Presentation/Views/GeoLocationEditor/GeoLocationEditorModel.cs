using System.ComponentModel;
using System.Windows.Data;
using AE.AuditPlanning.Logic.Models;
using AE.AuditPlanning.Presentation.Base;
using AE.AuditPlanning.Presentation.Common;

namespace AE.AuditPlanning.Presentation.Views.GeoLocationEditor
{
    public class GeoLocationEditorModel : ViewModelBase
    {
        private GeoLocationModel selectedGeoLocation;
        private string separatorSign;
        private string postalCodeCitySearchText;
        private int addToDbProgress;
        private bool isWorking;

        public GeoLocationEditorModel()
        {
            this.GeoLocations = new SmartCollection<GeoLocationModel>();

            this.GeoLocationsCollectionView = CollectionViewSource.GetDefaultView(this.GeoLocations);
            this.GeoLocationsCollectionView.SortDescriptions.Add(new SortDescription("PostalCode", ListSortDirection.Ascending));
            this.GeoLocationsCollectionView.Filter = o => ((GeoLocationModel)o).PostalCodeDisplay.StartsWith(this.PostalCodeCitySearchText) || ((GeoLocationModel)o).City.StartsWith(this.PostalCodeCitySearchText);
            
            this.PostalCodeCitySearchText = string.Empty;
        }

        public ICollectionView GeoLocationsCollectionView { get; private set; }

        public SmartCollection<GeoLocationModel> GeoLocations { get; set; }

        public string PostalCodeCitySearchText
        {
            get
            {
                return this.postalCodeCitySearchText;
            }
            set
            {
                this.postalCodeCitySearchText = value;
                this.GeoLocationsCollectionView.Refresh();
                this.OnPropertyChanged();
            }
        }

        public GeoLocationModel SelectedGeoLocation
        {
            get
            {
                return this.selectedGeoLocation;
            }
            set
            {
                this.selectedGeoLocation = value;
                this.OnPropertyChanged();
            }
        }

        public string SeparatorSign
        {
            get
            {
                return this.separatorSign;
            }
            set
            {
                this.separatorSign = value;
                this.OnPropertyChanged();
            }
        }

        public int AddToDbProgress
        {
            get
            {
                return this.addToDbProgress;
            }
            set
            {
                this.addToDbProgress = value;
                this.OnPropertyChanged();
            }
        }

        public bool IsWorking
        {
            get
            {
                return this.isWorking;
            }
            set
            {
                this.isWorking = value;
                this.OnPropertyChanged();
            }
        }
    }
}