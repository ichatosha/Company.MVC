using Company.Route.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        // IEnum to return more than one Employee 
        IEnumerable<Employee> GetByName(string name);

        //IEnumerable<Employee> GetAll();

        //Employee GetById(int id);

        //int Add(Employee employee);

        //int Update(Employee employee);

        //int Delete(Employee employee);



    }
}
