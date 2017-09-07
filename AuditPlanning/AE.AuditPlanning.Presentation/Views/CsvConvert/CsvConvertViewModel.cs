using System.Collections.ObjectModel;
using AE.AuditPlanning.Logic.Models;
using AE.AuditPlanning.Presentation.Base;

namespace AE.AuditPlanning.Presentation.Views.CsvConvert
{
    public class CsvConvertViewModel : ViewModelBase
    {
        private string filePath;
        private char seperator;

        public CsvConvertViewModel()
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

        public char Seperator
        {
            get
            {
                return this.seperator;
            }
            set
            {
                if (value == this.seperator)
                {
                    return;
                }

                this.seperator = value;
                this.OnPropertyChanged();
            }
        }
    }
}