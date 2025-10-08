using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using TodoList.Commands;
using TodoList.Models;
using TodoList.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Data;

namespace TodoList.ViewModels {
    public enum SortField {
        Status,
        Date,
        Title
    }

    public class MainViewModel : INotifyPropertyChanged {
        private readonly ITodoService _todoService;
        private string _newItemTitle = string.Empty;

        public ObservableCollection<TodoItem> TodoItems { get; }
        public ICollectionView TodoItemsView { get; }
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

        private SortField _selectedSortField = SortField.Status;
        public SortField SelectedSortField {
            get => _selectedSortField;
            set { 
                _selectedSortField = value; 
                OnPropertyChanged();
                UpdateSort();
            }
        }

        private bool _isSortAscending = true;
        public bool IsSortAscending {
            get => _isSortAscending;
            set { 
                _isSortAscending = value; 
                OnPropertyChanged();
                UpdateSort();
            }
        }

        public IEnumerable<SortField> SortFields => Enum.GetValues<SortField>();
        public string[] SortOrders => new[] { "По возрастанию", "По убыванию" }; 

        public MainViewModel(ITodoService todoService) {
            _todoService = todoService;
            TodoItems = new ObservableCollection<TodoItem>();
            TodoItemsView = CollectionViewSource.GetDefaultView(TodoItems);
            
            LoadItems();
            UpdateSort();

            AddCommand = new RelayCommand(AddNewItem, () => !string.IsNullOrWhiteSpace(NewItemTitle));
            ToggleCompleteCommand = new RelayCommand<TodoItem>(ToggleItem);
            RemoveCommand = new RelayCommand<TodoItem>(RemoveItem);
        }

        private void LoadItems()
        {
            TodoItems.Clear();
            foreach (var item in _todoService.GetAll())
            {
                TodoItems.Add(item);
            }
        }

        private void UpdateSort()
        {
            if (TodoItemsView == null) return;

            TodoItemsView.SortDescriptions.Clear();
            var sortProperty = SelectedSortField switch {
                SortField.Status => nameof(TodoItem.IsCompleted),
                SortField.Date => nameof(TodoItem.CreatedAt),
                SortField.Title => nameof(TodoItem.Title),
                _ => nameof(TodoItem.CreatedAt)
            };
            var direction = IsSortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;
            TodoItemsView.SortDescriptions.Add(new SortDescription(sortProperty, direction));
            TodoItemsView.Refresh();
        }

        private void AddNewItem() {
            if (string.IsNullOrWhiteSpace(NewItemTitle)) return;

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
            if (item == null) return;
            
            _todoService.Update(item);
            TodoItemsView.Refresh();
        }

        private void RemoveItem(TodoItem item) {
            if (item == null) return;
            _todoService.Remove(item.Id);
            TodoItems.Remove(item);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}