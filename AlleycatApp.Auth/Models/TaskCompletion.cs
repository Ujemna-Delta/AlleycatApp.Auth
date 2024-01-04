﻿using System.ComponentModel.DataAnnotations;
using AlleycatApp.Auth.Models.Users;

namespace AlleycatApp.Auth.Models
{
    public class TaskCompletion : IModel<int>
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }

        [Required] public string AttendeeId { get; set; } = null!;
        public Attendee Attendee { get; set; } = null!;

        public int TaskId { get; set; }
        public TaskModel Task { get; set; } = null!;
    }
}
