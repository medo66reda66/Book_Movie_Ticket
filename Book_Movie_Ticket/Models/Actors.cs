using Book_Movie_Ticket.Models;

namespace Book_Movie_Tictet.Models
{
    public class Actors
    {
        public int Id { get; set; }
        public string FullName { get; set; }= string.Empty;
        public string Img { get; set; }
        public List<ActorsMovie> ActorsMovies { get; set; } 
    }
}
