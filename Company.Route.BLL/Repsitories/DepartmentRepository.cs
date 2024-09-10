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
    public class DepartmentRepository : IDepartmentRespstory
    {
        // no can edit the values readonly
        private readonly AppDbContext _context;

        // ask CLR to create object from AppDbContext
        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Department> GetAll()
        {
            return _context.Departments.ToList();
        }

        public Department? GetById(int? id)
        {
            //return _context.Departments.FirstOrDefault(D => D.Id == id);
            return _context.Departments.Find(id);
        }

        public Department? GetByName(string? name)
        {
            return _context.Departments.Find(name);
        }

        public int Add(Department entity)
        {
            _context.Departments.Add(entity);
            // reuturn int 
            return _context.SaveChanges();
        }

        public int Update(Department entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(Department entity)
        {
            throw new NotImplementedException();
        }


    }
}
