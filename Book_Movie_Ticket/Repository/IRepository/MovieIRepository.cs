using Book_Movie_Tickets.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_Movie_Ticket.Repository.IRepository
{
    public interface MovieIRepository : IRepository<Movie>
    {
        void RemoveRange(IEnumerable<Movie> movies);
      
    }
}
