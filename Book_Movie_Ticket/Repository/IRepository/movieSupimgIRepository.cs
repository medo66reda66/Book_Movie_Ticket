using Book_Movie_Ticket.Models;
using Book_Movie_Tictet.data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Book_Movie_Ticket.Repository.IRepository
{
    public interface movieSupimgIRepository :IRepository<MovieSupimg>
    {
         void RemoveRange(IEnumerable<MovieSupimg> movies);

    }
}
