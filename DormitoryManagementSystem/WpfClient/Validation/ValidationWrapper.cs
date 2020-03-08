using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WpfClient.Validation
{
    public class ValidationWrapper<T> : NotifyDataErrorInfoBase
    {
        public ValidationWrapper(T model)
        {
            Model = model;
        }

        public T Model { get; set; }

        public bool ValidateModel(params string[] omitProperties)
        {
            ClearAllErrors();

            var properties = typeof(T).GetProperties()
                .Select(x => new { x.Name, Value = x.GetValue(Model) });

            foreach (var property in properties)
            {
                if (omitProperties.Contains(property.Name))
                    continue;

                ValidatePropertyInternal(property.Name, property.Value);
            }


            return !HasErrors;
        }

        protected void ValidatePropertyInternal(string propertyName, object currentValue)
        {
            ClearErrors(propertyName);
            ValidateCustomErrors(propertyName);
            ValidateDataAnnotations(propertyName, currentValue);
        }

        protected virtual TValue GetValue<TValue>([CallerMemberName]string propertyName = null)
        {
            return (TValue)typeof(T).GetProperty(propertyName).GetValue(Model);
        }

        protected virtual void SetValue<TValue>(TValue propertyValue, [CallerMemberName]string propertyName = null)
        {
            typeof(T).GetProperty(propertyName).SetValue(Model, propertyValue);
            NotifyOfPropertyChange(propertyName);
            ValidatePropertyInternal(propertyName, propertyValue);
        }

        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }

        private void ValidateDataAnnotations(string propertyName, object currentValue)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(Model) { MemberName = propertyName };
            Validator.TryValidateProperty(currentValue, context, validationResults);

            foreach (var error in validationResults)
            {
                AddError(propertyName, error.ErrorMessage);
            }
        }

        private void ValidateCustomErrors(string propertyName)
        {
            var errors = ValidateProperty(propertyName);

            if (errors != null && errors.Any(x => x != null))
            {
                foreach (var error in errors)
                {
                    AddError(propertyName, error);
                }
            }
        }
    }
}
