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
    public partial class DDataContextPage : ContentControl
    {
        public DDataContextPage()
        {
            InitializeComponent();
        }
    }

    class DDataContextViewModel : MyBindableBase
    {
        public string Message { get; } = "ViewModel の Message プロパティです。";
    }

    // デザイン用の static クラスを定義して使用する（View の d:DataContext に設定する）
    internal static class Design
    {
        public static DDataContextViewModel DDataContextViewModel { get; } = new();
    }
}
