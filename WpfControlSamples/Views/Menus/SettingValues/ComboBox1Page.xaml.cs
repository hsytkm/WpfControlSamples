﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class ComboBox1Page : ContentControl
    {
        public ComboBox1Page()
        {
            InitializeComponent();

            DataContext = new ComboBox1ViewModel();
        }
    }

    class ComboBox1ViewModel : MyBindableBase
    {
        private static IList<(string Name, Color Color)> Colors =>
            Models.SampleData.WpfColors;

        #region ComboBox1
        public IList<string> ItemsSource { get; } = Colors.Select(x => x.Name).ToList();

        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    var x = Colors.First(x => x.Name == value);
                    SelectedNameColor = new ComboBox1Color(x.Name, x.Color);
                }
            }
        }
        private string _selectedItem;
        #endregion

        #region ComboBox2
        public IList<ComboBox1Color> ItemsSourceNameColor { get; } =
            Colors.Select(x => new ComboBox1Color(x.Name, x.Color)).ToList();

        public ComboBox1Color SelectedNameColor
        {
            get => _selectedNameColor;
            set => SetProperty(ref _selectedNameColor, value);
        }
        private ComboBox1Color _selectedNameColor;
        #endregion

        public ComboBox1ViewModel()
        {
            SelectedItem = ItemsSource.First();
            SelectedNameColor = ItemsSourceNameColor.First();
        }
    }

    class ComboBox1Color
    {
        public string Name { get; }
        public Color Color { get; }
        public ComboBox1Color(string name, Color color) =>
            (Name, Color) = (name, color);
        public override string ToString() => Name;
    }
}
