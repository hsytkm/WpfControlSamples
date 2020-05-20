using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
    public partial class ApplicationCommandsPage : ContentControl
    {
        public ApplicationCommandsPage()
        {
            InitializeComponent();
        }

        #region ApplicationCommands
        public void CancelPrintMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void SelectAllMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void SaveAsMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void SaveMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void ReplaceMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void RedoMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void PropertiesMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void PrintPreviewMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void PrintMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void PasteMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void StopMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void OpenMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void NewMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void HelpMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void FindMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void DeleteMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void CutMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void CorrectionListMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void CopyMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void ContextMenuMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void CloseMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void NotACommandMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        public void UndoMethod(object sender, ExecutedRoutedEventArgs e) => ShowMessageBox();
        #endregion

        public void CommonCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = checkBox.IsChecked ?? false;
        }

        private static void ShowMessageBox([CallerMemberName]string caller = "UnknownMethod")
        {
            MessageBox.Show("Called " + caller);
        }

    }
}