using System.Windows;
using System.Windows.Markup;

/* WPF �� XmlnsDefinition (xaml �� Debug/Release �r���h���Q�Ƃ���j�́A
 * �ʃv���W�F�N�g���Q�Ƃ����ꍇ�łȂ��Ɠ��삵�Ȃ��炵���̂ŁA�ʃv���W�F�N�g�ɒ�`����B
 * 
 * see BuildConditionalPage.xaml
 * 
 * c# - WPF AlternateContent not working - Stack Overflow https://stackoverflow.com/questions/36150073/wpf-alternatecontent-not-working
 */

#if DEBUG
[assembly: XmlnsDefinition("debug-mode", "WpfControlSamples")]
#endif
