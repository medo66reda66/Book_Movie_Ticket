using Book_Movie_Ticket.data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Book_Movie_Ticket.Repository.IRepository
{
    public interface IRepository<T>where T : class
    {

        Task<T> AddAsync(T entity, CancellationToken cancellationToken);

        void Updat(T entity);

        void Delete(T entity);

        Task<IEnumerable<T>> GetAllAsync(
           Expression<Func<T, bool>> exception = null,
           Expression<Func<T, object>>?[] includ = null,
           CancellationToken cancellationToken = default,
           bool tracked = true
           );


        Task<T> GetoneAsync(
            Expression<Func<T, bool>> exception = null,
           Expression<Func<T, object>>?[] includ = null,
           CancellationToken cancellationToken = default,
           bool tracked = true);


        Task commitASync(CancellationToken cancellationToken);
      
    }
}
