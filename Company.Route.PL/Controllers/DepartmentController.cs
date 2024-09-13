using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repsitories;
using Company.Route.DAL.Data.Contexts;
using Company.Route.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.Route.PL.Controllers
{
    public class DepartmentController : Controller
    {
        // make object from this interface 
        private readonly IDepartmentRespstory _departmentRepository;
        
        private readonly AppDbContext _context;
        
        // ask CLR to create object from DepartmentRepository
        public DepartmentController(AppDbContext context , IDepartmentRespstory departmentRepository)
        {
            _context = context;
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var AllDepartments = _departmentRepository.GetAll();
            return View(AllDepartments);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        // decline any outside token can edit in web Like : Postman 
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department model)
        {
            if (ModelState.IsValid)
            {
                if (model.DateOfCreation == null)
                {
                    model.DateOfCreation = DateTime.Now;
                }

                var create = _departmentRepository.Add(model);

                if (create > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Details(int? id , string viewName = "Details")
        {
            // Bad Request
            if (id is null) return BadRequest(); // 400
            var result = _departmentRepository.GetById(id.Value);
            // Not Found
            if(result is null)
            {
                return NotFound(); // 404
            }
            return View(viewName ,result);
        }

        // Update
        [HttpGet]
        public IActionResult Update(int? id)
        {
            //// Bad Request
            //if (id is null) return BadRequest();

            //var result = _departmentRepository.GetById(id.Value);

            //if (result is null)
            //{
            //    return NotFound();
            //}

            //return View(result);

            // less the repetition of code
            return Details(id , "Update");
        }

        [HttpPost]
        // decline any outside token can edit in web Like : Postman 
        [ValidateAntiForgeryToken]
        // [FromRoute] >> to take the id from segment only not from form
        public IActionResult Update([FromRoute] int? id , Department UpdatedDepartment)
        {
            if (id != UpdatedDepartment.Id) return BadRequest(); // 400

            if(ModelState.IsValid)
            {
                var result = _departmentRepository.Update(UpdatedDepartment);
                if(result > 0 )
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            // else : stay in view
            
                return View(UpdatedDepartment);
            


            //////////////
            //var result = _context.Departments.SingleOrDefault(D => D.Id == UpdatedDepartment.Id);
            //if (result is null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    result.Name = UpdatedDepartment.Name;
            //    result.DateOfCreation = UpdatedDepartment.DateOfCreation;
            //    _context.SaveChanges();
            //}
            //return  View(UpdatedDepartment);
        }

        public IActionResult Update()
        {
            var departments = _context.Departments.ToList();
            return View(departments);
        }
        // Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id is null) { return BadRequest(); } // 400

            //var result = _departmentRepository.GetById(id.Value);

            //if (result is null)
            //{
            //    return NotFound(); // 404
            //}
            //return View(result);

            // lessw the repetition of code
            return Details(id, "Delete");

        }

        

        [HttpPost]
        //decline any outside token can edit in web Like : Postman 
        [ValidateAntiForgeryToken]
        // [FromRoute] >> to take the id from segment only not from form
        public IActionResult Delete([FromRoute] int? id , Department deletedDepartment)
        {
            if (id is null) return BadRequest(); // 400

            if (id != deletedDepartment.Id) return BadRequest(); // 400

            if (ModelState.IsValid)
            {
                var result = _departmentRepository.Delete(deletedDepartment);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            // else : stay in view
            return View(deletedDepartment);

        }
    }

}
