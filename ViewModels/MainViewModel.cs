using System.ComponentModel;
using System.Runtime.CompilerServices;
using TodoList.Commands;
using TodoList.Models;
using TodoList.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TodoList.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ITodoService _todoService;
        private string _newItemTitle = string.Empty;

        public ObservableCollection<TodoItem> TodoItems { get; }
        public string NewItemTitle {
            get => _newItemTitle;
            set {
                _newItemTitle = value;
                OnPropertyChanged();
            }
        }


        public ICommand AddCommand { get; }
        public ICommand ToggleCompleteCommand { get; }
        public ICommand RemoveCommand { get; }


        public MainViewModel(ITodoService todoService) {
            _todoService = todoService;
            TodoItems = new ObservableCollection<TodoItem>(_todoService.GetAll());
            AddCommand = new RelayCommand(AddNewItem, () => !string.IsNullOrWhiteSpace(NewItemTitle));
            ToggleCompleteCommand = new RelayCommand<TodoItem>(ToggleItem);
            RemoveCommand = new RelayCommand<TodoItem>(RemoveItem);
        }

        private void AddNewItem() {
            var newItem = new TodoItem {
                Id = Guid.NewGuid(),
                Title = NewItemTitle.Trim(),
                IsCompleted = false,
                CreatedAt = DateTime.Now
            };
            _todoService.Add(newItem);
            TodoItems.Add(newItem);
            NewItemTitle = string.Empty;
        }

        private void ToggleItem(TodoItem item) {
            item.IsCompleted = !item.IsCompleted;
            _todoService.Update(item);
        }

        private void RemoveItem(TodoItem item) {
            _todoService.Remove(item.Id);
            TodoItems.Remove(item);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
