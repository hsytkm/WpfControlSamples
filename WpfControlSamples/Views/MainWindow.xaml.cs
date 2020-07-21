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
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }

    class MainWindowViewModel : MyBindableBase
    {
        public TabItemPageParent[] TabItemPageParents { get; }

        public TabItemPageParent SelectedTabItemPageParent
        {
            get => _selectedTabItemPage;
            set
            {
                var oldPage = _selectedTabItemPage;
                if (SetProperty(ref _selectedTabItemPage, value))
                {
                    oldPage?.ReleaseContent();
                    _selectedTabItemPage?.LoadContent();
                }
            }
        }
        private TabItemPageParent _selectedTabItemPage;

        public MainWindowViewModel()
        {
            var allPageTables = PagesSource.AllPageTables;
            TabItemPageParents = allPageTables.Select(table => new TabItemPageParent(table)).ToArray();

            SelectedTabItemPageParent = TabItemPageParents.First();
        }
    }
}
