using Company.Route.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.BLL.Interfaces
{
    public interface IDepartmentRespstory
    {
        IEnumerable<Department> GetAll();

        Department GetById(int? id);

        Department GetByName(string name);

        // return int cause of the func save.changes return int
        int Add(Department entity);

        int Update(Department entity);

        int Delete(Department entity);

    }
}
