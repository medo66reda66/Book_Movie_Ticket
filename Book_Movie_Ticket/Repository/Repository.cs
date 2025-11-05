using Book_Movie_Ticket.Repository.IRepository;
using Book_Movie_Ticket.data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Book_Movie_Ticket.Repository
{
    public class Repository<T> :IRepository<T>  where T : class
    {

        private ApplicationDBContext _context ;
        private DbSet<T> _db;

        public Repository(ApplicationDBContext context)
        {
            _context = context;
            _db = _context.Set<T>();
        }

       

        public async Task<T> AddAsync(T entity ,CancellationToken cancellationToken)
        {
           await _db.AddAsync(entity,cancellationToken);
            return entity;
        }
        public void Updat(T entity)
        {
            _db.Update(entity);
        }
        public void Delete(T entity)
        {
            _db.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> exception = null,
            Expression<Func<T, object>>?[] includ = null,
            CancellationToken cancellationToken=default,
            bool tracked = true
            )
        {
            var entities = _db.AsQueryable();
            if (exception is not null)
            {
                entities = entities.Where(exception);
            }
            if (includ is not null )
            {
                foreach(var item1 in includ )
                {
                        entities = entities.Include(item1);
                }
            }
           
            if (!tracked)
            {
                entities = entities.AsNoTracking();
            }
              return  await  entities.ToListAsync(cancellationToken);
        }

        public async Task<T> GetoneAsync(
             Expression<Func<T, bool>> exception = null,
            Expression<Func<T, object>>?[] includ = null,
            CancellationToken cancellationToken = default,
            bool tracked = true)
        {
            return (await GetAllAsync(exception,includ,cancellationToken,tracked)).FirstOrDefault();
        }

        public async Task commitASync( CancellationToken cancellationToken)
        {
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error" + ex.Message);
            }

        }
    }
}
