using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using AE.AuditPlanning.Presentation.Properties;

namespace AE.AuditPlanning.Presentation.Base
{
    public class ViewControllerBase<TModel> : INotifyPropertyChanged where TModel : ViewModelBase, new()
    {
        private static bool? isInDesignMode;

        private readonly TModel model;

        public ViewControllerBase()
        {
            this.model = new TModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TModel Model
        {
            get
            {
                return this.model;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static bool IsInDesignModeStatic
        {
            get
            {
                if (!isInDesignMode.HasValue)
                {
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;

                    if (!isInDesignMode.Value && Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal))
                    {
                        isInDesignMode = true;
                    }
                }

                return isInDesignMode.Value;
            }
        }

        public bool IsInDesignMode
        {
            get
            {
                return IsInDesignModeStatic;
            }
        }
    }
}