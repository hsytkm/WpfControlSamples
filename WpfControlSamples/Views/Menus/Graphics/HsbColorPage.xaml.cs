#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
using WpfControlSamples.Models;

namespace WpfControlSamples.Views.Menus
{
    public partial class HsbColorPage : ContentControl
    {
        public HsbColorPage()
        {
            InitializeComponent();
            DataContext = new HsbColorViewModel();
        }
    }

    class HsbColorViewModel : MyBindableBase
    {
        public Color MyRgbColor
        {
            get => _myRgbColor;
            private set => SetProperty(ref _myRgbColor, value);
        }
        private Color _myRgbColor = Color.FromRgb(0, 0, 0);

        public HsbColor MyHsbColor
        {
            get => _myHsbColor;
            private set
            {
                if (SetProperty(ref _myHsbColor, value))
                    MyComplementaryColor = _myHsbColor.ToComplementaryColor();
            }
        }
        private HsbColor _myHsbColor;

        // 補色
        public HsbColor MyComplementaryColor
        {
            get => _myComplementaryColor;
            private set => SetProperty(ref _myComplementaryColor, value);
        }
        private HsbColor _myComplementaryColor;

        #region RGB/HSB
        public int Red
        {
            get => _red;
            set => SetProperty(ref _red, value);
        }
        private int _red;

        public int Green
        {
            get => _green;
            set => SetProperty(ref _green, value);
        }
        private int _green;

        public int Blue
        {
            get => _blue;
            set => SetProperty(ref _blue, value);
        }
        private int _blue;

        public int Hue
        {
            get => _hue;
            set => SetProperty(ref _hue, value);
        }
        private int _hue;

        public int Satu
        {
            get => _satu;
            set => SetProperty(ref _satu, value);
        }
        private int _satu;

        public int Bri
        {
            get => _bri;
            set => SetProperty(ref _bri, value);
        }
        private int _bri;
        #endregion

        public HsbColorViewModel()
        {
            this.PropertyChanged += (_, e) => DebugWriteLine($"PropertyChanged {e.PropertyName}");

            this.PropertyChanged += OnRgbColorPropertyChanged;
            this.PropertyChanged += OnHsbColorPropertyChanged;
            this.PropertyChanged += OnColorChannelsPropertyChanged;

#if false   // OK
            for (int r = 0; r <= 255; r++)
            {
                for (int g = 0; g <= 255; g++)
                {
                    for (int b = 0; b <= 255; b++)
                    {
                        var source = Color.FromRgb((byte)r, (byte)g, (byte)b);
                        var hsb = source.ToHsb();
                        var rgb = hsb.ToRgb();
                        Debug.Assert(source == rgb);
                    }
                }
            }
#endif
#if false   // Hsb から Rgb の変換で値が落ちるので、元の値が復元されない… しゃーない
            for (int hh = 0; hh <= 360; hh++)
            {
                for (int hs = 0; hs <= 100; hs++)
                {
                    for (int hb = 0; hb <= 100; hb++)
                    {
                        var source = HsbColor.FromHSB(hh, hs / 100d, hb / 100d);
                        var rgb = source.ToRgb();
                        var hsb = rgb.ToHsb();

                        Debug.Assert(source.Hue == Math.Round(hsb.Hue));
                        Debug.Assert(source.Saturation * 100d == Math.Round(hsb.Saturation * 100d, 0));
                        Debug.Assert(source.Brightness * 100d == Math.Round(hsb.Brightness * 100d, 0));
                    }
                }
            }
#endif
        }

        private void OnRgbColorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(MyRgbColor)) return;
            DebugWriteLine($"OnRgbColorPropertyChanged");

            // RGB ⇔ HSB の PropertyChanged が循環して StackOverflow にならないようにイベントをデタッチしてから更新する
            this.PropertyChanged -= OnHsbColorPropertyChanged;
            this.PropertyChanged -= OnColorChannelsPropertyChanged;

            MyHsbColor = MyRgbColor.ToHsbColor();
            UpdateColorChannels(MyRgbColor, MyHsbColor);

            this.PropertyChanged += OnHsbColorPropertyChanged;
            this.PropertyChanged += OnColorChannelsPropertyChanged;

            DebugWriteLine($"OnRgbColorPropertyChanged : Rgb->Hsb: {MyRgbColor} -> {MyHsbColor}");
        }

        private void OnHsbColorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(MyHsbColor)) return;
            DebugWriteLine($"OnHsbColorPropertyChanged");

            // RGB ⇔ HSB の PropertyChanged が循環して StackOverflow にならないようにイベントをデタッチしてから更新する
            this.PropertyChanged -= OnRgbColorPropertyChanged;
            this.PropertyChanged -= OnColorChannelsPropertyChanged;

            MyRgbColor = MyHsbColor.ToRgbColor();
            UpdateColorChannels(MyRgbColor, MyHsbColor);

            this.PropertyChanged += OnRgbColorPropertyChanged;
            this.PropertyChanged += OnColorChannelsPropertyChanged;

            DebugWriteLine($"OnHsbColorPropertyChanged : Hsb->Rgb: {MyHsbColor} -> {MyRgbColor}");
        }

        private void UpdateColorChannels(Color rgbColor, HsbColor hsbColor)
        {
            Red = rgbColor.R;
            Green = rgbColor.G;
            Blue = rgbColor.B;

            Hue = (int)hsbColor.Hue;
            Satu = (int)Math.Floor(hsbColor.Saturation * 100d);
            Bri = (int)Math.Floor(hsbColor.Brightness * 100d);
        }

        private void OnColorChannelsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Red):
                case nameof(Green):
                case nameof(Blue):
                    DebugWriteLine($"OnRgbPropertyChanged");
                    MyRgbColor = Color.FromRgb((byte)Red, (byte)Green, (byte)Blue);
                    break;
                case nameof(Hue):
                case nameof(Satu):
                case nameof(Bri):
                    DebugWriteLine($"OnHsbPropertyChanged");
                    MyHsbColor = HsbColor.FromHsb(Hue, Math.Round(Satu / 100d, 2), Math.Round(Bri / 100d, 2));
                    break;
            }
        }

        private static void DebugWriteLine(string s)
        {
            //Debug.WriteLine(s);
        }
    }
}
