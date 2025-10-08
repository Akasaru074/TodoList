namespace TodoList.Models {
    public class TodoItem {
        public required Guid Id { get; set; }
        public required string Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}