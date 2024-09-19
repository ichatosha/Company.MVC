using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.BLL.Interfaces
{
    // Generic Repo to refactoring the code
    public interface IGenericRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task<int> AddAsync(T employee);

        Task<int> UpdateAsync(T employee);

        Task<int> DeleteAsync(T employee);

    }
}
