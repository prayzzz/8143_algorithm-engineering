using AE.AuditPlanning.Logic.Models;
using AE.AuditPlanning.Logic.ViewServiceInterfaces;
using AE.AuditPlanning.Logic.ViewServices;
using AE.AuditPlanning.Storage.Entities;
using AutoMapper;
using Microsoft.Practices.Unity;

namespace AE.AuditPlanning.Logic
{
    public class ServiceLocator
    {
        private static readonly IUnityContainer Container;

        static ServiceLocator()
        {
            Container = new UnityContainer();
            Register(Container);
            CreateMaps();
        }

        public static ICsvConvertViewService CsvConvert
        {
            get
            {
                return Container.Resolve<ICsvConvertViewService>();
            }
        }

        public static IJsonLoaderViewService JsonLoader
        {
            get
            {
                return Container.Resolve<IJsonLoaderViewService>();
            }
        }

        public static IGraphBuilderViewService GraphBuilder
        {
            get
            {
                return Container.Resolve<IGraphBuilderViewService>();
            }
        }

        public static IGeoLocationEditorViewService GeoLocationEditor
        {
            get
            {
                return Container.Resolve<IGeoLocationEditorViewService>();
            }
        }

        public static void CleanUp()
        {
            Container.Dispose();
        }

        private static void CreateMaps()
        {
            Mapper.CreateMap<CustomerModel, Customer>();
            Mapper.CreateMap<Customer, CustomerModel>();

            Mapper.CreateMap<GeoLocation, GeoLocationModel>();
            Mapper.CreateMap<GeoLocationModel, GeoLocation>();
        }

        private static void Register(IUnityContainer builder)
        {
            builder.RegisterType<ICsvConvertViewService, CsvConvertViewService>();
            builder.RegisterType<IJsonLoaderViewService, JsonLoaderViewService>();
            builder.RegisterType<IGraphBuilderViewService, GraphBuilderViewService>();
            builder.RegisterType<IGeoLocationEditorViewService, GeoLocationEditorViewService>();
            builder.RegisterType<ICsvConvertViewService, CsvConvertViewService>();
            builder.RegisterType<ICsvConvertViewService, CsvConvertViewService>();
        }
    }
}
