using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Caliburn.Micro;

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

        public void AddError(string propName, string error)
        {
            if (!ErrorsByProperty.ContainsKey(propName))
                ErrorsByProperty[propName] = new List<string>();

            if (!ErrorsByProperty[propName].Contains(error))
            {
                ErrorsByProperty[propName].Add(IoC.Get<ResourceDictionary>("language")[error].ToString());
                RaiseErrorChangedEvent(propName);
            }
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

        protected void RaiseErrorChangedEvent(string propName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propName));
            NotifyOfPropertyChange(nameof(HasErrors));
        }
    }
}
