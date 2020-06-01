﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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
    public partial class FileDropPage : ContentControl
    {
        public FileDropPage()
        {
            InitializeComponent();
        }
    }

    class FileDropViewModel : MyBindableBase
    {
        public ObservableCollection<string> DroppedPaths { get; } = new ObservableCollection<string>();

        public FileDropViewModel()
        {
            DroppedPaths.CollectionChanged += DroppedPaths_CollectionChanged;
        }

        private void DroppedPaths_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(sender is ObservableCollection<string> collection)) return;

            //switch (e.Action)
            //{
            //    case NotifyCollectionChangedAction.Add:
            //        break;
            //}

            DroppedPathsText = string.Join(Environment.NewLine, collection.Select(x => x));
        }

        public ICommand ClearFilePathCommand => _clearFilePathCommand ??= new MyCommand(
                () => DroppedPaths.Clear(),
                () => DroppedPaths.Any());
        private ICommand _clearFilePathCommand;

        public string DroppedPathsText
        {
            get => _droppedPathsText;
            set
            {
                if (SetProperty(ref _droppedPathsText, value))
                    (ClearFilePathCommand as MyCommand).ChangeCanExecute();
            }
        }
        private string _droppedPathsText;
    }

}