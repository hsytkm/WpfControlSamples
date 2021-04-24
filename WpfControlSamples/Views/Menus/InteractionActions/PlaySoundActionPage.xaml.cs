#nullable disable
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
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
    public partial class PlaySoundActionPage : ContentControl
    {
        public PlaySoundActionPage()
        {
            InitializeComponent();
            DataContext = new PlaySoundActionViewMode();
        }
    }

    class PlaySoundActionViewMode : MyBindableBase
    {
        private readonly Uri _resourceWavSoundUri =
            new(@"pack://application:,,,/WpfControlSamples;component/Resources/Sounds/Resource1.wav");

        public ICommand PlayResouceWavCommand =>
            _playResouceWavCommand ??= new MyCommand(() =>
                PlayResourceSound(_resourceWavSoundUri));
        private ICommand _playResouceWavCommand;

        public ICommand PlayEmbeddedResouceWavCommand =>
            _playEmbeddedResouceWavCommand ??= new MyCommand(() =>
                PlayEmbeddedResourceSound("Resources.Sounds.EmbeddedResource1.wav"));
        private ICommand _playEmbeddedResouceWavCommand;

        private static void PlaySound(Stream stream)
        {
            if (stream is null) throw new NullReferenceException();
            using var player = new SoundPlayer(stream);
            player.Play();
        }

        private static void PlayResourceSound(Uri uri)
        {
            var sri = Application.GetResourceStream(uri);
            if (sri?.Stream != null)
            {
                using var stream = sri.Stream;
                PlaySound(stream);
            }
        }

        private static void PlayEmbeddedResourceSound(string resName)
        {
            static Stream GetEmbeddedResourceWavSoundStream(string resName)
            {
                var myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                var prjName = "WpfControlSamples";
                var fullResName = prjName + "." + resName;
                return myAssembly.GetManifestResourceStream(fullResName);
            }

            using var stream = GetEmbeddedResourceWavSoundStream(resName);
            PlaySound(stream);
        }

    }
}
