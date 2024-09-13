using LikeTours.Data.Common.Model;
using LikeTours.Data.DTO;
using LikeTours.Data.Enums;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LikeTours.Contracts.Repositories
{
    public interface IGenericRepository<T> where T : class , IDBEntity
    {
        Task<IEnumerable<T>> GetAllAsync(
          QueryParams param,
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           OrderType orderType = OrderType.Ascending,
           params Expression<Func<T, object>>[] includes);

        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
        Task<T> GetByIdAsync(int id, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);
        Task<int> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> NameExistsAsync(string name);
        Task<IEnumerable<T>> GetByColumnsAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);
    }
}
