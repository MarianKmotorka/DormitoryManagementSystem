using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using static Library.Models.ResultModel;

namespace WpfClient.Validation
{
    public class NotifyDataErrorInfoBase : Screen, INotifyDataErrorInfo
    {
        public bool HasErrors => ErrorsByProperty.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public Dictionary<string, List<string>> ErrorsByProperty { get; set; } = new Dictionary<string, List<string>>();

        public IEnumerable GetErrors(string propertyName)
        {
            return ErrorsByProperty.ContainsKey(propertyName)
                ? ErrorsByProperty[propertyName]
                : null;
        }

        public void AddError(string propertyName, string message, object customState = null)
        {
            if (!ErrorsByProperty.ContainsKey(propertyName))
                ErrorsByProperty[propertyName] = new List<string>();

            var translatedErrorMessage = $"{IoC.Get<ResourceDictionary>("language")[message]} {customState}";

            if (!ErrorsByProperty[propertyName].Contains(translatedErrorMessage))
            {
                ErrorsByProperty[propertyName].Add(translatedErrorMessage);
                RaiseErrorChangedEvent(propertyName);
            }
        }

        public void AddError(ErrorDetail error)
        {
            if (!ErrorsByProperty.ContainsKey(error.PropertyName))
                ErrorsByProperty[error.PropertyName] = new List<string>();

            var translatedErrorMessage = $"{IoC.Get<ResourceDictionary>("language")[error.Message]} {error.CustomState}";

            if (!ErrorsByProperty[error.PropertyName].Contains(translatedErrorMessage))
            {
                ErrorsByProperty[error.PropertyName].Add(translatedErrorMessage);
                RaiseErrorChangedEvent(error.PropertyName);
            }
        }

        public void AddErrors(IEnumerable<ErrorDetail> errors)
        {
            foreach (var error in errors)
                AddError(error);
        }

        public void ClearErrors(string propName)
        {
            if (ErrorsByProperty.ContainsKey(propName))
            {
                ErrorsByProperty.Remove(propName);
                RaiseErrorChangedEvent(propName);
            }
        }

        public void ClearAllErrors()
        {
            var propNames = ErrorsByProperty.Select(x => x.Key).ToList();
            ErrorsByProperty.Clear();
            propNames.ForEach(x => RaiseErrorChangedEvent(x));
        }

        public void RaiseErrorChangedEvent(string propName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propName));
            NotifyOfPropertyChange(nameof(HasErrors));
        }
    }
}
