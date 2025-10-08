using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using TodoList.Commands;
using TodoList.Models;
using TodoList.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TodoList.ViewModels
{

    public enum SortField {
        Status,
        Date,
        Title
    }

    public enum SortOrder {
        Ascending,
        Descending
    }

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


        private SortField _selectedSortField = SortField.Status;
        public SortField SelectedSortField {
            get => _selectedSortField;
            set { _selectedSortField = value; OnPropertyChanged(); RefreshSortedItems(); }
        }

        private SortOrder _sortOrder = SortOrder.Ascending;
        public SortOrder SortOrder {
            get => _sortOrder;
            set { _sortOrder = value; OnPropertyChanged(); RefreshSortedItems(); }
        }

        private bool _isSortAscending = true;
        public bool IsSortAscending {
            get => _isSortAscending;
            set { _isSortAscending = value; OnPropertyChanged(); RefreshSortedItems(); }
        }

        public IEnumerable<SortField> SortFields => Enum.GetValues<SortField>();
        public string[] SortOrders = new[] { "По возрастанию", "По убыванию" };


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

        private void RefreshSortedItems() {
            var items = _todoService.GetAll().AsQueryable();

            items = SelectedSortField switch {
                SortField.Status => IsSortAscending ? items.OrderBy(x => x.IsCompleted) : items.OrderByDescending(x => x.IsCompleted),
                SortField.Date => IsSortAscending ? items.OrderBy(x => x.CreatedAt) : items.OrderByDescending(x => x.CreatedAt),
                SortField.Title => IsSortAscending ? items.OrderBy(x => x.Title) : items.OrderByDescending(x => x.Title),
                _ => items.OrderBy(x => x.CreatedAt)
            };

            TodoItems.Clear();
            foreach (var item in items) {
                TodoItems.Add(item);
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
