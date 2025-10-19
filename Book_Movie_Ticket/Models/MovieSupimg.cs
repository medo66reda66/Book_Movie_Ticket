using Book_Movie_Tickets.Models;

namespace Book_Movie_Ticket.Models
{
    public class MovieSupimg
    {
        public int Id { get; set; }
        public string? SupImg { get; set; }
        public int? MovieId { get; set; }
        public Movie? Movie { get; set; }


    }
}
