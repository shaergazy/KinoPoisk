﻿using DAL.Models;
using DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class MoviePerson
    {
        [Key]
        public int Id { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
        public PersonType PersonType { get; set; } 
        public uint Order { get; set; }

    }
}
