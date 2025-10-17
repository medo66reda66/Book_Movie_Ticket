using Book_Movie_Ticket.Models;
using Book_Movie_Tictet.Models;

namespace Book_Movie_Tickets.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MainImg { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public bool Status { get; set; }
        public DateTime Datetime { get; set; }
        public int CinemaId { get; set; }
        public Cinema? Cinema { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<ActorsMovie> ActorsMovies { get; set; }
    }
}