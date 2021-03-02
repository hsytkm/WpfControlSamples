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
using System.Windows.Threading;
using WpfControlSamples.Extensions;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class DispatcherObjectPage : ContentControl
    {
        public DispatcherObjectPage()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // UIスレッドからの普通の呼び出しなのでOK
            var d = new DrivedObject();

            DataContext = "OKButton: " + d.GetData();
        }

        private async void NGButton_Click(object sender, RoutedEventArgs e)
        {
            // UIスレッド以外(Task)からの呼び出しなので例外が出る
            var d = new DrivedObject();

            DataContext = "NGButton: " + await Task.Run(() => d.GetData());
        }

        private async void DispatcherButton_Click(object sender, RoutedEventArgs e)
        {
            // UIスレッド以外(Task)だがDispatcher経由での呼び出しなのでOK
            var d = new DrivedObject();

            await Task.Run(async () =>
            {
                /* DispatcherObject.CheckAccess()
                 *  現スレッドが DispatcherObject に紐付けられたスレッドかチェックし、
                 *  UIスレッドでなければ false を返す
                 */
                if (!d.CheckAccess())
                {
                    await d.Dispatcher.InvokeAsync(() =>
                    {
                        // DispatcherObjectからのデータ取得も、DataContextの書き替えも両方OK
                        DataContext = "DispatcherButton: " + d.GetData();
                    });
                }
            });
        }
    }

    class DrivedObject : DispatcherObject
    {
        public string GetData()
        {
            try
            {
                /* DispatcherObject.VerifyAccess()
                 *  現スレッドが DispatcherObject に紐付けられたスレッドかチェックし、
                 *  UIスレッドでなければ InvalidOperationException をスローする
                 */
                this.VerifyAccess();

                return "Success!";
            }
            catch (InvalidOperationException ex)
            {
                return ex.GetType().ToString();
            }
        }
    }
}
