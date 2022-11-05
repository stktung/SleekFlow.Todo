﻿using SleekFlow.Todo.Domain.Common;
using SleekFlow.Todo.Domain.Event;

namespace SleekFlow.Todo.Domain.Projection
{
    public class TodoProjection
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Completed { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public long LastEventNumber { get; set; }

        private void Apply(Common.DomainEvent e)
        {
            switch (e)
            {
                case TodoCreatedEvent evt:
                    Apply(evt);
                    break;
                case TodoNameTextInsertedEvent evt:
                    Apply(evt);
                    break;
            }
        }

        private void Apply(TodoCreatedEvent e)
        {
            Id = e.Id;
            Name = null;
            Description = null;
            DueDate = null;
            Completed = false;
            LastUpdatedAt = e.RaisedAt;
            LastEventNumber = e.EventNumber;
        }

        private void Apply(TodoNameTextInsertedEvent e)
        {
            if (e.Position < 0)
                throw new ProjectionException($"Position must be greater than or equals to 0. Position:'{e.Position}'");
            if (string.IsNullOrEmpty(Name) && e.Position > 0)
                throw new ProjectionException(
                    $"Position not exceed length of name. Position: '{e.Position}' Name length: '0'");
            if (!string.IsNullOrEmpty(Name) && e.Position >= Name.Length)
                throw new ProjectionException(
                    $"Position not exceed length of name. Position: '{e.Position}' Name length: '{Name.Length}'");

            Name = string.IsNullOrEmpty(Name) ? e.Text : Name.Insert(e.Position, e.Text);

            LastUpdatedAt = e.RaisedAt;
            LastEventNumber = e.EventNumber;
        }


        public static TodoProjection Load(IEnumerable<Common.DomainEvent> events)
        {
            var todo = new TodoProjection();

            foreach (var e in events)
            {
                todo.Apply(e);
            }

            return todo;
        }
    }
}
