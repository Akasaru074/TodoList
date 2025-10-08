using System.Collections.Generic;
using TodoList.Models;

namespace TodoList.Services
{
    public class InMemoryTodoService : ITodoService
    {
        private readonly List<TodoItem> _items = new();

        public IEnumerable<TodoItem> GetAll() => _items;
        public void Add(TodoItem item) => _items.Add(item);
        public void Update(TodoItem updatedItem) {
            var existing = _items.FirstOrDefault(i => i.Id == updatedItem.Id);
            if (existing != null) {
                existing.Title = updatedItem.Title;
                existing.IsCompleted = updatedItem.IsCompleted;
            }
        }
        public void Remove(Guid id) => _items.RemoveAll(i => i.Id == id);

    }
}
