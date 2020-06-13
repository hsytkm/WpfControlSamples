using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfControlSamples.Infrastructures
{
    abstract class MyBindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        //protected virtual bool SetPropertyWithDispose<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        //{
        //    if (Equals(field, value)) return false;
        //    if (field is IDisposable d) d.Dispose();

        //    return SetProperty(ref field, value, propertyName);
        //}

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
