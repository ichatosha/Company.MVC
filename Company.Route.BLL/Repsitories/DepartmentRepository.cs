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
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRespstory
    {
        // no can edit the values readonly
        //private readonly AppDbContext _context;
        

        // ask CLR to create object from AppDbContext
        public DepartmentRepository(AppDbContext context) : base(context)
        {
            
        }

        //public IEnumerable<Department> GetAll()
        //{
        //    return _context.Departments.ToList();
        //}

        //public Department? GetById(int? id)
        //{
        //    // Find EF
        //    //return _context.Departments.FirstOrDefault(D => D.Id == id);
        //    return _context.Departments.Find(id);
        //}

        //public Department? GetByName(string? name)
        //{
        //    // Find EF
        //    return _context.Departments.Find(name);
        //}

        //public int Add(Department entity)
        //{
        //    // Add EF
        //    _context.Departments.Add(entity);
        //    // reuturn int 
        //    return _context.SaveChanges();
        //}

        //public int Update(Department entity)
        //{
        //    // Update EF
        //    _context.Departments.Update(entity);
        //    return _context.SaveChanges();
        //}

        //public int Delete(Department entity)
        //{
        //    // remove EF
        //    _context.Departments.Remove(entity);
        //    return _context.SaveChanges();
        //}


    }
}
