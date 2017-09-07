using System;
using System.Collections.Generic;
using System.Linq;
using AE.AuditPlanning.Logic;
using AE.AuditPlanning.Logic.Common;
using AE.AuditPlanning.Logic.Models;
using AE.AuditPlanning.Logic.ViewServiceInterfaces;
using AE.AuditPlanning.Presentation.Base;
using AE.AuditPlanning.Presentation.Common;

namespace AE.AuditPlanning.Presentation.Views.CsvConvert
{
    public class CsvConvertViewController : ViewControllerBase<CsvConvertViewModel>
    {
        private readonly ICsvConvertViewService service;

        public CsvConvertViewController()
        {
            this.service = ServiceLocator.CsvConvert;

            this.BrowseCommand = new RelayCommand(this.Browse);

            this.LoadCsvCommand = new RelayCommand(this.LoadCsv, this.CanLoadCsv);
            this.FilterPostalCodeCommand = new RelayCommand(this.FilterPostalCode);
            this.MinimizeCommand = new RelayCommand(this.Minimize);
            this.SaveToJsonCommand = new RelayCommand(this.SaveToJson);

            this.Model.Seperator = ';';
        }

        public RelayCommand MinimizeCommand { get; set; }

        public RelayCommand FilterPostalCodeCommand { get; set; }

        public RelayCommand BrowseCommand { get; private set; }

        public RelayCommand LoadCsvCommand { get; private set; }

        public RelayCommand SaveToJsonCommand { get; private set; }

        private void Browse()
        {
            this.Model.FilePath = FileDialogHelper.OpenFileDialog(this.Model.FilePath, ".csv", "Kommaliste (*.csv)|*.csv");
        }

        private void LoadCsv()
        {
            this.Model.Customers.Clear();
            foreach (var customer in this.service.LoadCsv(this.Model.FilePath, this.Model.Seperator))
            {
                this.Model.Customers.Add(customer);
            }
        }

        private bool CanLoadCsv()
        {
            if (string.IsNullOrEmpty(this.Model.FilePath) || !this.service.FileExists(this.Model.FilePath))
            {
                return false;
            }

            return true;
        }

        private void SaveToJson()
        {
            string path;
            if (FileDialogHelper.SaveFileDialog(this.Model.FilePath, ".json", "JSON Datei (*.json)|*.json", out path))
            {
                this.service.Save(path, this.Model.Customers);
            }
        }

        #region Obsolete

        private void FilterPostalCode()
        {
            this.Model.Customers.RemoveAll(x => !x.PostalCode.StartsWith("0") && !x.PostalCode.StartsWith("1"));
        }

        private void Minimize()
        {
            var groups = this.Model.Customers.GroupBy(x => x.City).ToList();

            var filteredByCity = groups.Select(g => g.First()).ToList();

            var list = new List<CustomerModel>();

            var r = new Random();
            while (list.Count < 300)
            {
                var item = filteredByCity[r.Next(filteredByCity.Count)];
                filteredByCity.Remove(item);
                list.Add(item);
            }

            this.Model.Customers.Clear();
            foreach (var customerViewModel in list)
            {
                this.Model.Customers.Add(customerViewModel);
            }
        }
        #endregion
    }
}