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
        IEnumerable<T> GetAll();

        T GetById(int id);

        int Add(T employee);

        int Update(T employee);

        int Delete(T employee);


    }
}
