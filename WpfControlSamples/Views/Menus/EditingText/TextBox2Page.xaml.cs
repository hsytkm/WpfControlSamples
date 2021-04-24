#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using WpfControlSamples.Infrastructures;

namespace WpfControlSamples.Views.Menus
{
    public partial class TextBox2Page : System.Windows.Controls.ContentControl
    {
        public TextBox2Page()
        {
            InitializeComponent();
            DataContext = new TextBox2ViewModel();
        }
    }

    class TextBox2ViewModel : MyBindableBase, IDataErrorInfo
    {
        [Required(ErrorMessage = "Required")]
        [Range(1, 100, ErrorMessage = "1から100で入力してください。")]
        public int? Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }
        private int? _number = 0;

        public static ValidationResult ValidateName(string test, ValidationContext context)
        {
            if (string.IsNullOrEmpty(test)) return ValidationResult.Success;

            if (test.StartsWith("test"))
                return ValidationResult.Success;

            return new ValidationResult("test で始まってほしい");
        }

        public string Error => throw new NotImplementedException();
        public string this[string columnName]
        {
            get
            {
                var value = GetValue(columnName);
                var validationContext = new ValidationContext(this) { MemberName = columnName };
                var results = new List<ValidationResult>();

                if (!Validator.TryValidateProperty(value, validationContext, results))
                {
                    foreach (var item in results)
                        return item.ErrorMessage;
                }
                return null;
            }
        }

        public object GetValue(string propName) => GetValue(propName, this);

        private static object GetValue(string propName, object model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));
            var prop = model.GetType().GetProperty(propName, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);
            return prop.GetValue(model);
        }
    }

    class NumericValidation : System.Windows.Controls.ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value is string s && !string.IsNullOrWhiteSpace(s))
            {
                if (!int.TryParse(s, out _))
                    return new System.Windows.Controls.ValidationResult(false, "整数を入力してください。");
            }

            return new System.Windows.Controls.ValidationResult(true, null);
        }
    }

}
