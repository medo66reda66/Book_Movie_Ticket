using Book_Movie_Ticket.Models;
using Book_Movie_Tickets.Models;

namespace Book_Movie_Ticket.MovieVM
{
    public class PromotionVM
    {

        public Promotion Promotion { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
    }
}

