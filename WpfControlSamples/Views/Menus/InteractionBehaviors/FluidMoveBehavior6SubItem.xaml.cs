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
    public partial class FluidMoveBehavior6SubItem : UserControl
    {
        // 本Viewのアイテム連番(先頭=0)
        public static readonly DependencyProperty ItemIndexProperty =
            DependencyProperty.Register(nameof(ItemIndex), typeof(int), typeof(FluidMoveBehavior6SubItem));
        public int ItemIndex
        {
            get => (int)GetValue(ItemIndexProperty);
            set => SetValue(ItemIndexProperty, value);
        }

        // アイテム総数
        public static readonly DependencyProperty ItemsCountProperty =
            DependencyProperty.Register(nameof(ItemsCount), typeof(int), typeof(FluidMoveBehavior6SubItem));
        public int ItemsCount
        {
            get => (int)GetValue(ItemsCountProperty);
            set => SetValue(ItemsCountProperty, value);
        }

        // アイテム入替コマンド
        public static readonly DependencyProperty SwapItemCommandProperty =
            DependencyProperty.Register(nameof(SwapItemCommand), typeof(ICommand), typeof(FluidMoveBehavior6SubItem));
        public ICommand SwapItemCommand
        {
            get => (ICommand)GetValue(SwapItemCommandProperty);
            set => SetValue(SwapItemCommandProperty, value);
        }

        public FluidMoveBehavior6SubItem()
        {
            InitializeComponent();
            DataContext = new FluidMoveBehavior6SubItemViewModel();
        }
    }

    class FluidMoveBehavior6SubItemViewModel : MyBindableBase
    {
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
        private bool _isEnabled = true;

        public string DirectoryPath
        {
            get => _directoryPath;
            set => SetProperty(ref _directoryPath, value);
        }
        private string _directoryPath;

        public bool IsReferenceModel
        {
            get => _isReferenceModel;
            set => SetProperty(ref _isReferenceModel, value);
        }
        private bool _isReferenceModel;
    }
}
