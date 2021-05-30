using System;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.ViewModels
{
    class NameValuePair<T> : MyBindableBase, IUpdatableItem<NameValuePair<T>>
    {
        public string Name { get; }
        public T Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
        private T _value = default!;

        public NameValuePair(string name, T value) => (Name, Value) = (name, value);

        public virtual bool IsSameKey(NameValuePair<T> other) => this.Name == other.Name;

        public virtual void UpdateValue(NameValuePair<T> other) => this.Value = other.Value;
    }

    // わざわざ定義する必要ないけど、可読性のために定義してみた
    class NameDoublePair : NameValuePair<double>, IUpdatableItem<NameDoublePair>
    {
        public NameDoublePair(string name, double value) : base(name, value) { }

        public bool IsSameKey(NameDoublePair other) => base.IsSameKey(other);
        public void UpdateValue(NameDoublePair other) => base.UpdateValue(other);
    }
}
