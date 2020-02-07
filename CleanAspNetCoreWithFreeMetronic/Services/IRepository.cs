using CleanAspNetCoreWithFreeMetronic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanAspNetCoreWithFreeMetronic.Services
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int? id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        //Task<bool> UpdateAsync2(T entity);
        bool ExistAsync(int id);
        //Task<T> DeleteAsync(int id);
        Task<bool> DeleteAsync(int id);
        //Task<T> DeleteAsync(int id);
    }
}
