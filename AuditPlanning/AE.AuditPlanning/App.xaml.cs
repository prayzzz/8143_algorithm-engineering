using System.Windows;
using AE.AuditPlanning.Storage;
using AE.AuditPlanning.Storage.Repositories;

namespace AE.AuditPlanning
{
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            GeoLocationRepository.Current.Shutdown();
        }
    }
}
