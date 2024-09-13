using LikeTours.Contracts.Repositories;
using LikeTours.Data;
using LikeTours.Data.Common.Model;
using LikeTours.Data.DTO;
using LikeTours.Data.DTO.Place;
using LikeTours.Data.Enums;
using LikeTours.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace LikeTours.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class , IDBEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly int max_page_size ;


        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            max_page_size = 100;
        }

        public async Task<IEnumerable<T>> GetAllAsync(
          QueryParams param,
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           OrderType orderType = OrderType.Ascending,
           params Expression<Func<T, object>>[] includes)
         {
            IQueryable<T> query = _dbSet;

            
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

        
            if (orderBy != null)
            {
                query = orderBy(query);
            }
          
            else if (orderType == OrderType.Descending)
            {
                query = query.OrderByDescending(e => e.Id);
            }
            else
            {
                query = query.OrderBy(e => e.Id);

            }

            if (param.PageSize > max_page_size)
            {
                param.PageSize = max_page_size;
            }

           if(param.HasPagination)
            {

              query = query.Skip((param.Page - 1) * param.PageSize).Take(param.PageSize);
            }

            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

           
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }


        public async Task<T> GetByIdAsync(int id,  Expression<Func<T, bool>> filter = null , params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task<IEnumerable<T>> GetByColumnsAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }


        public async Task<int> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            var entry = _context.Entry(entity);
            var idProperty = entry.Metadata.FindPrimaryKey().Properties.FirstOrDefault();
            return  (int)entry.Property(idProperty.Name).CurrentValue ;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
           
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await _dbSet.OfType<Place>().AnyAsync(p => p.Name == name);
        }

        
    }
}
