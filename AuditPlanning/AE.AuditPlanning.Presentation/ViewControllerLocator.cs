using AE.AuditPlanning.Presentation.Views.GeoLocationEditor;
using AE.AuditPlanning.Presentation.Views.GraphBuilder;
using AE.AuditPlanning.Presentation.Views.MainWindow;
using Microsoft.Practices.Unity;

namespace AE.AuditPlanning.Presentation
{
    public class ViewControllerLocator
    {
        private static readonly IUnityContainer Container;

        static ViewControllerLocator()
        {
            Container = new UnityContainer();

            Register(Container);
        }

        public static MainWindowController MainWindow
        {
            get
            {
                return Container.Resolve<MainWindowController>();
            }
        }

        public static GraphBuilderViewController GraphBuilder
        {
            get
            {
                return Container.Resolve<GraphBuilderViewController>();
            }
        }

        public static GeoLocationEditorController GeoLocation
        {
            get
            {
                return Container.Resolve<GeoLocationEditorController>();
            }
        }

        public static void CleanUp()
        {
            Container.Dispose();
        }

        private static void Register(IUnityContainer builder)
        {
            builder.RegisterType<MainWindowController>();
            builder.RegisterType<GraphBuilderViewController>();
            builder.RegisterType<GeoLocationEditorController>();
        }
    }
}