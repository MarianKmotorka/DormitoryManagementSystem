using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;

namespace WpfClient.Validation
{
    public class NotifyDataErrorInfoBase : Screen, INotifyDataErrorInfo
    {
        public bool HasErrors => _errorsByProperty.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private Dictionary<string, List<string>> _errorsByProperty { get; set; }
            = new Dictionary<string, List<string>>();

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByProperty.ContainsKey(propertyName)
                ? _errorsByProperty[propertyName]
                : null;
        }

        protected void AddError(string propName, string error)
        {
            if (!_errorsByProperty.ContainsKey(propName))
                _errorsByProperty[propName] = new List<string>();

            if (!_errorsByProperty[propName].Contains(error))
            {
                _errorsByProperty[propName].Add(error);
                RaiseErrorChangedEvent(propName);
            }
        }

        protected void ClearErrors(string propName)
        {
            if (_errorsByProperty.ContainsKey(propName))
            {
                _errorsByProperty.Remove(propName);
                RaiseErrorChangedEvent(propName);
            }
        }

        protected void RaiseErrorChangedEvent(string propName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propName));
            NotifyOfPropertyChange(nameof(HasErrors));
        }
    }
}
