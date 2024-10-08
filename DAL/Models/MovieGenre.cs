﻿using DAL.Models;

namespace DAL.Models
{
    public class MovieGenre
    {
        public int Id { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
