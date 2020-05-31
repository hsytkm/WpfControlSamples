using System;
using System.Collections.Generic;
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

namespace WpfControlSamples.Views.Menus
{
    public partial class CardView : ContentControl
    {
        public static readonly DependencyProperty BorderColorBrushProperty = DependencyProperty.Register(
            nameof(BorderColorBrush), typeof(Brush), typeof(CardView), new PropertyMetadata(Brushes.Gray));
        public Brush BorderColorBrush
        {
            get => (Brush)GetValue(BorderColorBrushProperty);
            set => SetValue(BorderColorBrushProperty, value);
        }

        public static readonly DependencyProperty CardColorBrushProperty = DependencyProperty.Register(
            nameof(CardColorBrush), typeof(Brush), typeof(CardView), new PropertyMetadata(Brushes.Black));
        public Brush CardColorBrush
        {
            get => (Brush)GetValue(CardColorBrushProperty);
            set => SetValue(CardColorBrushProperty, value);
        }

        public static readonly DependencyProperty TextColorBrushProperty = DependencyProperty.Register(
            nameof(TextColorBrush), typeof(Brush), typeof(CardView), new PropertyMetadata(Brushes.White));
        public Brush TextColorBrush
        {
            get => (Brush)GetValue(TextColorBrushProperty);
            set => SetValue(TextColorBrushProperty, value);
        }

        public static readonly DependencyProperty CardTitleProperty = DependencyProperty.Register(
            nameof(CardTitle), typeof(string), typeof(CardView), new PropertyMetadata(nameof(CardTitle)));
        public string CardTitle
        {
            get => (string)GetValue(CardTitleProperty);
            set => SetValue(CardTitleProperty, value);
        }

        public static readonly DependencyProperty CardDescriptionProperty = DependencyProperty.Register(
            nameof(CardDescription), typeof(string), typeof(CardView), new PropertyMetadata(nameof(CardDescription)));
        public string CardDescription
        {
            get => (string)GetValue(CardDescriptionProperty);
            set => SetValue(CardDescriptionProperty, value);
        }

        public CardView()
        {
            InitializeComponent();
        }
    }
}