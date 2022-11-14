using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Viewmodels
{
    public class ErrorViewModel : INotifyDataErrorInfo
    {
        public ErrorViewModel()
        {
            m_errorsByPropertyName = new Dictionary<string, List<string>>();
        }

        public bool HasErrors => m_errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return m_errorsByPropertyName.GetValueOrDefault(propertyName, null);
        }

        public IList<string> GetErroredProperties() => m_errorsByPropertyName.Keys.ToList();

        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!m_errorsByPropertyName.ContainsKey(propertyName))
                m_errorsByPropertyName.Add(propertyName, new List<string>());

            var propertyErrors = m_errorsByPropertyName[propertyName];
            if (propertyErrors.Contains(errorMessage))
                return;

            m_errorsByPropertyName[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }

        public void RemoveError(string propertyName)
        {
            if (m_errorsByPropertyName.Remove(propertyName))
                OnErrorsChanged(propertyName);
        }

        Dictionary<string, List<string>> m_errorsByPropertyName;
    }
}
