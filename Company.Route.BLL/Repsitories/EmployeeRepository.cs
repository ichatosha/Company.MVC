using Company.Route.BLL.Interfaces;
using Company.Route.DAL.Data.Contexts;
using Company.Route.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.BLL.Repsitories
{
    public class EmployeeRepository :GenericRepository<Employee>, IEmployeeRepository
    {
        //private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context) : base (context)
        {
            
        }

        //public IEnumerable<Employee> GetAll()
        //{
        //    return _context.Employees.ToList();
        //}

        //public Employee GetById(int id)
        //{
        //    // Find EF
        //    return _context.Employees.Find(id);
        //}

        //public int Add(Employee employee)
        //{
        //    _context.Employees.Add(employee);
        //    return _context.SaveChanges();
        //}

        //public int Update(Employee employee)
        //{
        //   _context.Employees.Update(employee);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Employee employee)
        //{
        //    _context.Employees.Remove(employee);
        //    return _context.SaveChanges();
        //}
    }
}
