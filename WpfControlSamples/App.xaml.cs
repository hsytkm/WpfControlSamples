using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using WpfControlSamples.Models;

namespace WpfControlSamples;

public partial class App : Application
{
    // AppクラスにModelインスタンスを持たせる
    // https://github.com/runceel/Livet-samples/blob/master/CommunicationBetweenViewModels/App.xaml.cs
    public static new App Current => (App)Application.Current;

    internal SampleModelContext SampleModelContext { get; } = new();

    private void App_OnStartup(object sender, StartupEventArgs e)
    {
        // App内で MainWindow のインスタンスを取得するためxamlで直設定しています
        MainWindow.Closing += static (_, _) => Debug.WriteLine("Call Dispose() here.");

        // https://zenn.dev/nuits_jp/articles/2023-03-08-wpf-unhandled-exception
    }

    #region DispatcherUnhandledException
    // ここで例外をハンドルしなければアプリ死にます
    // http://gushwell.ldblog.jp/archives/52335498.html
    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        var message = $"未ハンドルの例外を検出しましたー" + Environment.NewLine
            + $"{e.Exception.GetType()} : {e.Exception.Message}";

        MessageBox.Show(message, "Exception occurred", MessageBoxButton.OK, MessageBoxImage.Error);
        e.Handled = true;
    }

    public void RemoveEvent_DispatcherUnhandledException()
        => DispatcherUnhandledException -= App_DispatcherUnhandledException;
    #endregion
}
