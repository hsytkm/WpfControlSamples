using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WpfControlSamples.Models;

namespace WpfControlSamples
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // AppクラスにModelインスタンスを持たせる
        // https://github.com/runceel/Livet-samples/blob/master/CommunicationBetweenViewModels/App.xaml.cs
        public static new App Current => (App)Application.Current;

        internal SampleModelContext SampleModelContext { get; } = new SampleModelContext();

        #region DispatcherUnhandledException
        // ここで例外をハンドルしなければアプリ死にます
        // http://gushwell.ldblog.jp/archives/52335498.html
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var message = $"未ハンドルの例外を検出しましたー" + Environment.NewLine
                + $"{e.Exception.GetType()} : {e.Exception.Message}";

            MessageBox.Show(message, "Exception occurred", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        public void RemoveEvent_DispatcherUnhandledException()
            => DispatcherUnhandledException -= Application_DispatcherUnhandledException;
        #endregion

    }
}
