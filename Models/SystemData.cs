using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using WpfControlSamples.Extensions;

namespace WpfControlSamples.Models
{
    static class SystemData
    {
        /// <summary>
        /// System.Environmentのstaticプロパティ
        /// </summary>
        public static IList<NameValueKey> EnvironmentProperties
        {
            get
            {
                if (_environmentProperties is null)
                {
                    var list = new List<NameValueKey>();

                    var type = typeof(Environment);
                    var source = type.GetProperties(BindingFlags.Static | BindingFlags.Public);
                    foreach (var x in source)
                    {
                        try
                        {
                            var value = x.GetValue(null).ToString();
                            list.Add(new NameValueKey(x.Name, value));
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                    _environmentProperties = list;
                }
                return _environmentProperties;
            }
        }
        private static IList<NameValueKey> _environmentProperties;

        /// <summary>
        /// System.Environmentのstaticプロパティ
        /// </summary>
        public static IList<NameValueKey> EnvironmentDirectories
        {
            get
            {
                if (_environmentDirectories is null)
                {
                    var list = new List<NameValueKey>();

                    var type = typeof(Environment.SpecialFolder);
                    var source = MyArrayExtension.GetEnums<Environment.SpecialFolder>();
                    foreach (var x in source)
                    {
                        try
                        {
                            var name = Enum.GetName(type, x);
                            var value = Environment.GetFolderPath(x);
                            list.Add(new NameValueKey(name, value));
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                    _environmentDirectories = list;
                }
                return _environmentDirectories;
            }
        }
        private static IList<NameValueKey> _environmentDirectories;

        /// <summary>
        ///  System.Reflection.Assembly
        /// </summary>
        public static IList<NameValueKey> AssemblyList { get; } =
            new List<NameValueKey>()
            {
                new NameValueKey("AssemblyVersion", Assembly.GetExecutingAssembly().GetName().Version.ToString()),
            };

        /// <summary>
        /// System.Diagnostics.FileVersionInfo
        /// </summary>
        public static IList<NameValueKey> FileVersionInfoList
        {
            get
            {
                if (_fileVersionInfoList is null)
                {
                    var list = new List<NameValueKey>();

                    var type = typeof(FileVersionInfo);
                    var info = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                    var source = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    foreach (var x in source)
                    {
                        try
                        {
                            var value = x.GetValue(info).ToString();
                            list.Add(new NameValueKey(x.Name, value));
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                    _fileVersionInfoList = list;
                }
                return _fileVersionInfoList;
            }
        }
        private static IList<NameValueKey> _fileVersionInfoList;

    }
}
