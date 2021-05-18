#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfControlSamples.ViewModels;

namespace WpfControlSamples.Views.Menus
{
    public partial class ScrollViewer2Page : ContentControl
    {
        public ScrollViewer2Page()
        {
            DataContext = new ScrollViewer2ViewModel();
            InitializeComponent();
        }
    }

    class ScrollViewer2ViewModel : MyBindableBase
    {
        // ScrollViewer
        public ObservableCollection<NameDoublePair> ScrollViewerProperties { get; } = new();
        public IReadOnlyList<NameDoublePair> ScrollViewerPropertiesSource
        {
            get => _scrollViewerPropertiesSource;
            set
            {
                if (SetProperty(ref _scrollViewerPropertiesSource, value))
                    ScrollViewerProperties.AddOrUpdateRange(value);
            }
        }
        private IReadOnlyList<NameDoublePair> _scrollViewerPropertiesSource;

        // ContentPresenter
        public ObservableCollection<NameDoublePair> ContentPresenterProperties { get; } = new();
        public IReadOnlyList<NameDoublePair> ContentPresenterPropertiesSource
        {
            get => _contentPresenterPropertiesSource;
            set
            {
                if (SetProperty(ref _contentPresenterPropertiesSource, value))
                    ContentPresenterProperties.AddOrUpdateRange(value);
            }
        }
        private IReadOnlyList<NameDoublePair> _contentPresenterPropertiesSource;

        // HorizontalScrollBar
        public ObservableCollection<NameDoublePair> HorizontalScrollBarProperties { get; } = new();
        public IReadOnlyList<NameDoublePair> HorizontalScrollBarPropertiesSource
        {
            get => _horizontalScrollBarPropertiesSource;
            set
            {
                if (SetProperty(ref _horizontalScrollBarPropertiesSource, value))
                    HorizontalScrollBarProperties.AddOrUpdateRange(value);
            }
        }
        private IReadOnlyList<NameDoublePair> _horizontalScrollBarPropertiesSource;

        // VerticalScrollBar
        public ObservableCollection<NameDoublePair> VerticalScrollBarProperties { get; } = new();
        public IReadOnlyList<NameDoublePair> VerticalScrollBarPropertiesSource
        {
            get => _verticalScrollBarPropertiesSource;
            set
            {
                if (SetProperty(ref _verticalScrollBarPropertiesSource, value))
                    VerticalScrollBarProperties.AddOrUpdateRange(value);
            }
        }
        private IReadOnlyList<NameDoublePair> _verticalScrollBarPropertiesSource;

    }
}
