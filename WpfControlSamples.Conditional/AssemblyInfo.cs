using System.Windows;
using System.Windows.Markup;

/* WPF の XmlnsDefinition (xaml で Debug/Release ビルドを参照する）は、
 * 別プロジェクトを参照した場合でないと動作しないらしいので、別プロジェクトに定義する。
 * 
 * see BuildConditionalPage.xaml
 * 
 * c# - WPF AlternateContent not working - Stack Overflow https://stackoverflow.com/questions/36150073/wpf-alternatecontent-not-working
 */

#if DEBUG
[assembly: XmlnsDefinition("debug-mode", "WpfControlSamples")]
#endif
