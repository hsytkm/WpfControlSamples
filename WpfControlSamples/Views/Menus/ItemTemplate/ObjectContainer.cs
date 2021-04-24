#nullable disable
using System;

namespace WpfControlSamples.Views.Menus
{
    interface IObjectContainer
    {
        object Data { get; set; }
    }

    class ObjectContainer<T> : IObjectContainer
    {
        public T Source { get; }
        public object Data { get; set; }

        public ObjectContainer(T data)
        {
            Source = data;
            Data = data;
        }
    }
}
