using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repsitories;
using Company.Route.DAL.Data.Contexts;
using Company.Route.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Company.Route.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        private readonly IEmployeeRepository _employeeRepository;

        // ask CLR to create object from DepartmentRepository and AppDbContext
        public EmployeeController(AppDbContext context, IEmployeeRepository employeeRepository)
        {
            _context = context;
            _employeeRepository = employeeRepository;
        }

        #region Get All
        [HttpGet]
        public IActionResult Index()
        {
            var AllEmployees = _employeeRepository.GetAll();
            return View(AllEmployees);
        }
        #endregion


        #region Create
        // CREATE
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        // decline any outside token can edit in web Like : Postman 
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee model)
        {
            if (ModelState.IsValid)
            {
                if (model.DateOfCreation == null)
                {
                    model.DateOfCreation = DateTime.Now;
                }
                var result = _employeeRepository.Add(model);
                if (result > 0)
                {
                    return RedirectToAction("Index");
                }
                // else :  stay in model
            }
            return View(model);
        }
        #endregion


        #region Details
        [HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
        {

            if (id is null) return BadRequest(); //400

            var result = _employeeRepository.GetById(id.Value);

            if (result == null) return NotFound(); //404


            return View(ViewName, result);
        }

        #endregion


        #region Update
        [HttpGet]
        public IActionResult Update(int? id)
        {
            return Details(id, "Update");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Update(int? id, Employee UpdatedEmployee)
        {
            if (id != UpdatedEmployee.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                var result = _employeeRepository.Update(UpdatedEmployee);
                if (result > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            // else : stay in model
            return View(UpdatedEmployee);

        }

        public IActionResult Update()
        {
            var Employees = _context.Employees.ToList();
            return View(Employees);
        }

        #endregion


        #region Delete

        [HttpGet]
        public IActionResult Delete(int? id)
        {

            return Details(id, "Delete");
            
        }


        [HttpPost]
        //decline any outside token can edit in web Like : Postman 
        [ValidateAntiForgeryToken]
        // [FromRoute] >> to take the id from segment only not from form
        public IActionResult Delete([FromRoute] int? id , Employee DeletedEmployee)
        {
            if (id is null) return BadRequest();

            if (ModelState.IsValid)
            {
                var result = _employeeRepository.Delete(DeletedEmployee);
                if (result > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            // else : stay in model
            return View(DeletedEmployee);
        }
        #endregion



    }
}
