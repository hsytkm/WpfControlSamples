using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class NavigationServicePage : ContentControl
    {
        private readonly NavigationService _navigationService;

        private readonly List<Uri> _sourcePages = new List<Uri>()
        {
            new Uri("Views/Menus/LayoutSingle/NavigationServiceContent1Page.xaml", UriKind.Relative),
            new Uri("Views/Menus/LayoutSingle/NavigationServiceContent2Page.xaml", UriKind.Relative),
            new Uri("Views/Menus/LayoutSingle/NavigationServiceContent3Page.xaml", UriKind.Relative),
        };

        public NavigationServicePage()
        {
            InitializeComponent();

            _navigationService = myFrame.NavigationService;
        }

        private void Frame_Loaded(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigate(_sourcePages[0]);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            int index = _sourcePages.IndexOf(_navigationService.CurrentSource);
            int count = _sourcePages.Count;

            prevButton.IsEnabled = (0 < index);
            nextButton.IsEnabled = (index < count - 1);

            DataContext = $"{index + 1} / {count}";
        }

        private void Button_Click_Prev(object sender, RoutedEventArgs e)
        {
            var navi = _navigationService;
            var sourcePages = _sourcePages;

            if (navi.CanGoBack)
            {
                navi.GoBack();
            }
            else
            {
                int index = sourcePages.FindIndex(p => p == navi.CurrentSource);
                navi.Navigate(sourcePages[index - 1]);
            }
        }

        private void Button_Click_Next(object sender, RoutedEventArgs e)
        {
            var navi = _navigationService;
            var sourcePages = _sourcePages;

            if (navi.CanGoForward)
            {
                navi.GoForward();
            }
            else
            {
                int index = sourcePages.FindIndex(p => p == navi.CurrentSource);
                navi.Navigate(sourcePages[index + 1]);
            }
        }

    }
}
