using System.Collections.ObjectModel;
using AE.AuditPlanning.Logic.Models;
using AE.AuditPlanning.Presentation.Base;

namespace AE.AuditPlanning.Presentation.Views.JsonLoader
{
    public class JsonLoaderViewModel : ViewModelBase
    {
        private string filePath;

        private int taskProgress;

        public JsonLoaderViewModel()
        {
            this.Customers = new ObservableCollection<CustomerModel>();
        }

        public ObservableCollection<CustomerModel> Customers { get; private set; }

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

        public int TaskProgress
        {
            get
            {
                return this.taskProgress;
            }

            set
            {
                if (value == this.taskProgress)
                {
                    return;
                }

                this.taskProgress = value;
                this.OnPropertyChanged();
            }
        }
    }
}