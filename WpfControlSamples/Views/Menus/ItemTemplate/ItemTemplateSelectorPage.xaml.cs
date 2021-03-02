using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class ItemTemplateSelectorPage : ContentControl
    {
        public ItemTemplateSelectorPage()
        {
            InitializeComponent();

            var containers = new List<IObjectContainer>()
            {
                new ObjectContainer<bool>(false),
                new ObjectContainer<bool>(true),
                new ObjectContainer<int>(123),
                new ObjectContainer<string>("Hello"),
                new ObjectContainer<string>(""),
                new ObjectContainer<int>(4321),
                new ObjectContainer<string>("こんにちわ"),
                new ObjectContainer<bool>(false),
                new ObjectContainer<int>(0),
            };
            DataContext = containers;
        }
    }
}
