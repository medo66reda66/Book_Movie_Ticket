using Book_Movie_Ticket.Models;
using Book_Movie_Ticket.Repository.IRepository;
using Book_Movie_Tickets.Models;
using Book_Movie_Tictet.data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Book_Movie_Ticket.Repository
{
    public class movieSupimgRepository : Repository<MovieSupimg> ,movieSupimgIRepository
    {
        private ApplicationDBContext _context = new();
        public void RemoveRange(IEnumerable<MovieSupimg> movies)
        {
            _context.RemoveRange(movies);
        }
    }
    }

