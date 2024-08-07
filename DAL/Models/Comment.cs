﻿using DAL.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsPublished { get; set; }
        public Guid MovieId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
