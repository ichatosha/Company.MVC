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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        // Protected to use in classes that inherit from base 
        private protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }


        // Async return => void Or Task Or Task<>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Employee))
            {
                // AsNoTracking : More Secure
                return (IEnumerable<T>) await _context.Employees.Include(E => E.WorkFor).AsNoTracking().ToListAsync();
            }
           return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);

        }

        public async Task<int> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            // update has not Async
             _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(T entity)
        {
            // Delete has not Async
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

    }
}

