using BulkTest.Models;
using System.ComponentModel;

namespace UWPClient.Models
{
    public class RequestParameterBindable : RequestParameters, INotifyPropertyChanged
    {
        public override TestResult IsPassed
        {
            get => base.IsPassed;
            set
            {
                base.IsPassed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPassed)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set
            {
                _isRunning = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
            }
        }

        string _name = "Request";
        public string Name { get => _name; set { _name = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name))); } }

        EditMode _editMode;
        public EditMode EditMode { get => _editMode; set { _editMode = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditMode))); } }

        string _rawValue = null;
        public override string RawValue { get => _rawValue;
            set { _rawValue = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RawValue))); } }
    }

    public enum EditMode
    {
        Form,
        Raw
    }
}