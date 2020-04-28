using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

    }
}
