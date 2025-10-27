using Book_Movie_Ticket.Repository.IRepository;
using Book_Movie_Tickets.Models;
using Book_Movie_Tictet.data;
using System.Linq.Expressions;

namespace Book_Movie_Ticket.Repository
{
    public class MovieReposiory:Repository<Movie> , MovieIRepository
    {
        private ApplicationDBContext _context = new();
        public void RemoveRange(IEnumerable<Movie> movies)
        {
            _context.RemoveRange(movies);
        }

       
    }
}
