using Gooios.ImageService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Gooios.ImageService.Repositories
{
    public class Repository<E, K> : IRepository<E, K> where E : Entity<K>
    {
        DatabaseContext _verificationDbContext = null;

        private readonly DbSet<E> _dbset;

        public Repository(IDbContextProvider dbContextProvider)
        {
            _verificationDbContext = dbContextProvider.GetDbContext();
            _dbset = _verificationDbContext.Set<E>();
        }

        public void Add(E item)
        {
            _dbset.Add(item);
        }
        
        public E Get(K id)
        {
            return _dbset.Find(id);
        }

        public E Get(Expression<Func<E, bool>> where)
        {
            return _dbset.FirstOrDefault(where);
        }

        public IEnumerable<E> GetAll()
        {
            return _dbset;
        }

        public IEnumerable<E> GetFiltered(Expression<Func<E, bool>> filter)
        {
            return _dbset.Where(filter);
        }

        public IEnumerable<E> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<E, KProperty>> orderByExpression, bool ascending)
        {
            if (ascending)
            {
                return _dbset.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }
            else
            {
                return _dbset.OrderByDescending(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }
        }

        public void Update(E item)
        {
            _dbset.Attach(item);
            _verificationDbContext.Entry(item).State = EntityState.Modified;
        }

        public void Remove(E item)
        {
            _dbset.Remove(item);
        }
    }
}
