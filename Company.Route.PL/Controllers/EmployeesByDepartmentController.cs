using AutoMapper;
using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repsitories;
using Company.Route.DAL.Data.Contexts;
using Company.Route.DAL.Models;
using Company.Route.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Company.Route.PL.Controllers
{
	[Authorize]
	public class EmployeesByDepartmentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRespstory _departmentRespstory;
        private readonly IMapper _mapper;


        public EmployeesByDepartmentController(AppDbContext context, IEmployeeRepository employeeRepository,
            IDepartmentRespstory departmentRespstory,
            IMapper mapper)
        {
            _context = context;
            _employeeRepository = employeeRepository;
            _departmentRespstory = departmentRespstory;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult EmployeesByDepartment(int? departmentId)
        {
            if (departmentId is null) return BadRequest();
            //var AllEmployeesInThisDept = Enumerable.Empty<Employee>();
            //var employeeViewModels = new Collection<EmployeeViewModel>();
            var  AllEmployeesInThisDept = _context.Employees.Where(e => e.WorkForId == departmentId).ToList();
            
            //var AllEmployeesInThisDept = Enumerable.Empty<Employee>();
            
            if (AllEmployeesInThisDept.IsNullOrEmpty())
            {
                TempData["NoEmpInDept"] = "There is no Employees in this Department!";
                return RedirectToAction("Index", "Department");
            }
            var ViewModel = _mapper.Map<IEnumerable<EmployeeViewModel>>(AllEmployeesInThisDept);
           
            return View(ViewModel);
        }
    }
}

