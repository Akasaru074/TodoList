using TodoList.Models;

namespace TodoList.Services
{
    public interface ITodoService
    {
        IEnumerable<TodoItem> GetAll();
        void Add(TodoItem item);
        void Update(TodoItem item);
        void Remove(Guid id);
    }
}
