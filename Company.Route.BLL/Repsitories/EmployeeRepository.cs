using Company.Route.BLL.Interfaces;
using Company.Route.DAL.Data.Contexts;
using Company.Route.DAL.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Employee>> GetByNameAsync(string name)
        {
            return await _context.Employees.Where(E => E.Name.ToLower().Contains(name.ToLower())).Include(E => E.WorkFor).ToListAsync();
             
        }


        public IEnumerable<Employee> GetByNameByDept(int? id, string name)
        {
            var allEmployeesInThisDept = _context.Employees
                .Where(e => e.WorkForId == id &&
                           (string.IsNullOrEmpty(name) || e.Name.ToLower().Contains(name.ToLower())))
                .Include(e => e.WorkFor)
                .ToList();

            return allEmployeesInThisDept;
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
