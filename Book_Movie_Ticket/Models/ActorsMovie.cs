using Book_Movie_Tickets.Models;
using Book_Movie_Tictet.Models;

namespace Book_Movie_Ticket.Models
{
    public class ActorsMovie
    {
       public int MovieId { get; set; }
         public Movie? Movie { get; set; }
        public int ActorId { get; set; }
        public Actors? Actor { get; set; }


    }
}
