using Book_Movie_Ticket.Models;
using Book_Movie_Tickets.Models;
using Book_Movie_Tictet.Models;

namespace Book_Movie_Ticket.MovieVM
{
    public class MovieVM
    {
        public IEnumerable<Movie>? Movies { get; set; }
        public Movie? Movie { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Cinema>? Cinemas { get; set; }
        public IEnumerable<Actors>? Actors { get; set; }
        public IEnumerable<MovieSupimg>? MovieSupimg { get; set; }
        public MovieSupimg? MovieSupimgs { get; set; }
        public IEnumerable<ActorsMovie>? ActorsMovieS { get; set; }

    }
}
