#nullable disable
using System;
using System.Collections.Generic;
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
    public partial class EmbeddedFontPage : ContentControl
    {
        public EmbeddedFontPage()
        {
            InitializeComponent();

            //var glyphs = new[] { "\uf2b0", "\uf2b1", "\uf2b2", "\uf2b3", };

            var codeStart = 0xf100;
            var codeEnd = 0xf500;
            DataContext = Enumerable.Range((char)codeStart, (char)(codeEnd - codeStart))
                .Select(x => new GlyphData(x)).ToList();
        }
    }

    class GlyphData
    {
        public char Code { get; }
        public string String { get; }
        public GlyphData(int x)
        {
            Code = (char)x;
            String = $"\\u{x:x4}";
        }
    }
}
