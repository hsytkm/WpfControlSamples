#nullable disable
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Shell;

//<i:Interaction.Triggers>
//    <i:PropertyChangedTrigger Binding="{Binding ProgressRatio, Mode=OneWay}" >
//        <action:TaskbarProgressAction />
//    </i:PropertyChangedTrigger>
//</i:Interaction.Triggers>
namespace WpfControlSamples.Views.Actions
{
    /// <summary>
    /// タスクバーに進捗を表示する
    /// </summary>
    class TaskbarProgressAction : TriggerAction<FrameworkElement>
    {
        private TaskbarItemInfo _taskbarItemInfo;

        protected override void OnAttached()
        {
            base.OnAttached();

            // OnAttached() で取得しようとしてるけど null が返ってくる…
            // 実害ないので参考のために残しておく
            SetTaskbarItemInfo(this.AssociatedObject);
        }

        protected override void OnDetaching()
        {
            _taskbarItemInfo = null;

            base.OnDetaching();
        }

        protected override void Invoke(object parameter)
        {
            if (parameter is not DependencyPropertyChangedEventArgs e) return;
            if (e.NewValue is not double progress) return;

            // OnAttached() で取得しようとしたができなかったので…
            if (_taskbarItemInfo is null)
            {
                _taskbarItemInfo = SetTaskbarItemInfo(this.AssociatedObject);
            }

            SetTaskbarProgress(_taskbarItemInfo, progress);
        }

        private static TaskbarItemInfo SetTaskbarItemInfo(FrameworkElement fe)
        {
            if (Window.GetWindow(fe) is not Window window) return null;

            if (window.TaskbarItemInfo is null)
                window.TaskbarItemInfo = new TaskbarItemInfo();

            return window.TaskbarItemInfo;
        }

        private static void SetTaskbarProgress(TaskbarItemInfo taskbar, double progress)
        {
            if (taskbar is null) throw new ArgumentNullException(nameof(taskbar));

            // 100% が一瞬も表示されへんけど、まぁいいやろ
            if (progress < 0 || 1.0 <= progress)
            {
                taskbar.ProgressState = TaskbarItemProgressState.None;
                taskbar.ProgressValue = 0;
            }
            else
            {
                taskbar.ProgressState = TaskbarItemProgressState.Normal;
                taskbar.ProgressValue = progress;
            }
        }

    }
}
