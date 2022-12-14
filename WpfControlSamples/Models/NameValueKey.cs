using System;
using System.Collections.Generic;

namespace WpfControlSamples.Models
{
    class NameValueKey
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public NameValueKey(string name, string value) =>
            (Name, Value) = (name, value);

        public static readonly IReadOnlyList<NameValueKey> Data = new NameValueKey[]
        {
            new("Jotaro", "Star Platinum"),
            new("DIO", "World 21"),
            new("Joseph", "Hermit Purple"),
            new("Polnareff", "Silver Chariot"),
        };
    }
}
