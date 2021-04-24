#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            this.Loaded += MainWindow_Loaded;
        }

        #region CustomContextMenu
        //// Showing a Custom ContextMenu on a WPF Window Title Bar
        ////   https://brianlagunas.com/showing-a-custom-contextmenu-on-a-wpf-window-title-bar/

        //// http://www.pinvoke.net/default.aspx/Constants/WM.html
        //private const Int32 WM_NCRBUTTONDOWN = 0xa4;
        //private const Int32 HTCAPTION = 0x02;

        //private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    IntPtr windowHandle = new WindowInteropHelper(this).Handle;
        //    HwndSource hwndSource = HwndSource.FromHwnd(windowHandle);
        //    hwndSource.AddHook(new HwndSourceHook(WndProc));
        //}

        //private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        //{
        //    if ((msg == WM_NCRBUTTONDOWN) && (wParam.ToInt32() == HTCAPTION))
        //    {
        //        ShowContextMenu();
        //        handled = true;
        //    }
        //    return IntPtr.Zero;
        //}

        //private void ShowContextMenu()
        //{
        //    var contextMenu = Resources["myContextMenu"] as ContextMenu;
        //    contextMenu.IsOpen = true;
        //}

        //private void ContextMenu_Click(object sender, RoutedEventArgs e)
        //{
        //    var item = e.OriginalSource as MenuItem;
        //    MessageBox.Show($"{item.Header} was clicked.");
        //}
        #endregion

        #region AppendContextMenu
        // Append Menu Items to the WPF Window ContextMenu
        //   https://brianlagunas.com/append-menu-items-to-the-wpf-window-contextmenu/

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        //A window receives this message when the user chooses a command from the Window menu, or when the user chooses the maximize button, minimize button, restore button, or close button.
        private const Int32 WM_SYSCOMMAND = 0x112;

        //Draws a horizontal dividing line.This flag is used only in a drop-down menu, submenu, or shortcut menu.The line cannot be grayed, disabled, or highlighted.
        private const Int32 MF_SEPARATOR = 0x800;

        //Specifies that an ID is a position index into the menu and not a command ID.
        private const Int32 MF_BYPOSITION = 0x400;

        //Menu Ids for our custom menu items
        private const Int32 _ItemOneMenuId = 1000;
        private const Int32 _ItemTwoMenuId = 1001;

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr windowhandle = new WindowInteropHelper(this).Handle;
            HwndSource hwndSource = HwndSource.FromHwnd(windowhandle);

            //Get the handle for the system menu
            IntPtr systemMenuHandle = GetSystemMenu(windowhandle, false);

            //Insert our custom menu items
            InsertMenu(systemMenuHandle, 5, MF_BYPOSITION | MF_SEPARATOR, 0, string.Empty); //Add a menu seperator
            InsertMenu(systemMenuHandle, 6, MF_BYPOSITION, _ItemOneMenuId, "Item 1"); //Add a setting menu item
            InsertMenu(systemMenuHandle, 7, MF_BYPOSITION, _ItemTwoMenuId, "Item 2"); //add an About menu item

            hwndSource.AddHook(new HwndSourceHook(WndProc));
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Check if the SystemCommand message has been executed
            if (msg == WM_SYSCOMMAND)
            {
                //check which menu item was clicked
                switch (wParam.ToInt32())
                {
                    case _ItemOneMenuId:
                        MessageBox.Show("Item 1 was clicked");
                        handled = true;
                        break;
                    case _ItemTwoMenuId:
                        MessageBox.Show("Item 2 was clicked");
                        handled = true;
                        break;
                }
            }
            return IntPtr.Zero;
        }
        #endregion
    }

    class MainWindowViewModel : MyBindableBase, IDisposable
    {
        public TabItemPageParent[] TabItemPageParents { get; }

        public TabItemPageParent SelectedTabItemPageParent
        {
            get => _selectedTabItemPage;
            set
            {
                var oldPage = _selectedTabItemPage;
                if (SetProperty(ref _selectedTabItemPage, value))
                {
                    oldPage?.ReleaseContent();
                    _selectedTabItemPage?.LoadContent();
                }
            }
        }
        private TabItemPageParent _selectedTabItemPage;

        public MainWindowViewModel()
        {
            var allPageTables = PagesSource.AllPageTables;
            TabItemPageParents = allPageTables.Select(table => new TabItemPageParent(table)).ToArray();

            SelectedTabItemPageParent = TabItemPageParents.First();
        }

        public void Dispose()
        {
            // called from Closed event of window.
        }
    }
}
