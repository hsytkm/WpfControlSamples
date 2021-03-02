using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class CollectionView4Page : ContentControl
    {
        public CollectionView4Page()
        {
            InitializeComponent();
            DataContext = new CollectionView4ViewModel();
        }
    }

    class CollectionView4ViewModel : MyBindableBase
    {
        public ObservableCollection<CView4Person> People { get; } =
            new ObservableCollection<CView4Person>();

        // データ初期設定
        public ICommand CreatePeopleCommand => _createPeopleCommand ??=
            new MyCommand(() =>
            {
                InitPeople();
                SetSortDescription();
                EnableRealtimeSort();
            });
        private ICommand _createPeopleCommand;

        // 給料の更新
        public ICommand UpdateSalaryCommand => _updateSalaryCommand ??=
            new MyCommand(() =>
            {
                foreach (var p in People) { p.Salary = GetSalary(); }

                // データ更新時に都度Viewをリフレッシュすれば、
                // ICollectionViewLiveShaping を設定しなくてもソートできる
                //var view = CollectionViewSource.GetDefaultView(_people);
                //view.Refresh();
            });
        private ICommand _updateSalaryCommand;

        // 適当なデータを数件作成
        private void InitPeople()
        {
            var people = Enumerable.Range(0, 10)
                .Select(i => new CView4Person
                {
                    Name = "Name " + i,
                    Salary = GetSalary()
                });

            People.Clear();
            foreach (var p in people)
                People.Add(p);
        }

        // Salaryプロパティでソート
        private void SetSortDescription()
        {
            var view = CollectionViewSource.GetDefaultView(People);
            var sortDescription = new SortDescription(nameof(CView4Person.Salary), ListSortDirection.Descending);
            view.SortDescriptions.Add(sortDescription);
        }

        // リアルタイムソート
        private void EnableRealtimeSort()
        {
            var view = CollectionViewSource.GetDefaultView(People);

            // ICollectionViewLiveShapingを実装していない場合は何もしない
            if (!(view is ICollectionViewLiveShaping liveShaping)) return;

            // リアルタイムソートをサポートしている場合は対象のプロパティにSalaryを追加して有効化
            if (liveShaping.CanChangeLiveSorting)
            {
                liveShaping.LiveSortingProperties.Add(nameof(CView4Person.Salary));
                liveShaping.IsLiveSorting = true;
            }
        }

        private readonly Random _random = new Random();
        private int GetSalary() => _random.Next(500_000);
    }

    class CView4Person : MyBindableBase
    {
        public int Salary
        {
            get => _salary;
            set => SetProperty(ref _salary, value);
        }
        private int _salary;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private string _name;
    }
}
