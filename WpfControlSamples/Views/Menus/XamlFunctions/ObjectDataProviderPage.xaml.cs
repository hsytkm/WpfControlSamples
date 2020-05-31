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
    public partial class ObjectDataProviderPage : ContentControl
    {
        public ObjectDataProviderPage()
        {
            InitializeComponent();
        }
    }

    class PersonProvider1
    {
        public string FullName { get; set; }
        public PersonProvider1() { }
    }

    class PersonProvider2
    {
        public string FullName { get; }
        public PersonProvider2(string first, string family) => FullName = first + " " + family;
    }

    class PersonProvider3 : MyBindableBase
    {
        private readonly string _familyName;
        public string FullName
        {
            get => _fullName;
            private set => SetProperty(ref _fullName, value);
        }
        private string _fullName;
        public PersonProvider3(string family) => _familyName = family;
        public void SetFirstName(string first) => FullName = first + " " + _familyName;
    }
}