using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TodoList.Models {
    public class TodoItem : INotifyPropertyChanged {
        private Guid _id;
        private string _title = string.Empty;
        private bool _isCompleted;
        private DateTime _createdAt;

        public Guid Id {
            get => _id;
            set {
                if (_id != value) {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Title {
            get => _title;
            set {
                if (_title != value) {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsCompleted {
            get => _isCompleted;
            set {
                if (_isCompleted != value) {
                    _isCompleted = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime CreatedAt {
            get => _createdAt;
            set {
                if (_createdAt != value) {
                    _createdAt = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}