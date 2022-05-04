using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Shared
{
    public class Todo
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
        public DateTime Timestamp { get; set; }

        public Todo() { }
    }
}
