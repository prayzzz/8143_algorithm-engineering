using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AE.AuditPlanning.Controls.KeyValue
{
    public class KeyBoolControl : Control
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            "Key",
            typeof(string),
            typeof(KeyBoolControl),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(bool),
            typeof(KeyBoolControl),
            new FrameworkPropertyMetadata
            {
                DefaultValue = false,
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            });

        static KeyBoolControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyBoolControl), new FrameworkPropertyMetadata(typeof(KeyBoolControl)));
        }

        public string Key
        {
            get
            {
                return (string)this.GetValue(KeyProperty);
            }

            set
            {
                this.SetValue(KeyProperty, value);
            }
        }

        public bool Value
        {
            get
            {
                return (bool)this.GetValue(ValueProperty);
            }

            set
            {
                this.SetValue(ValueProperty, value);
            }
        }
    }
}