using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AE.AuditPlanning.Controls.KeyValue
{
    public class KeyValueListControl : Control
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            "Key",
            typeof(string),
            typeof(KeyValueListControl),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(int?),
            typeof(KeyValueListControl),
            new FrameworkPropertyMetadata
            {
                DefaultValue = null,
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            });

        public static readonly DependencyProperty ValueListProperty = DependencyProperty.Register(
            "ValueList",
            typeof(Dictionary<int, string>),
            typeof(KeyValueListControl),
            new PropertyMetadata(default(Dictionary<int, string>)));

        static KeyValueListControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KeyValueListControl), new FrameworkPropertyMetadata(typeof(KeyValueListControl)));
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

        public int? Value
        {
            get
            {
                return (int?)this.GetValue(ValueProperty);
            }

            set
            {
                this.SetValue(ValueProperty, value);
            }
        }

        public Dictionary<int, string> ValueList
        {
            get
            {
                return (Dictionary<int, string>)this.GetValue(ValueListProperty);
            }

            set
            {
                this.SetValue(ValueListProperty, value);
            }
        }
    }
}