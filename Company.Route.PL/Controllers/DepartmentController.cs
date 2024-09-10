using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repsitories;
using Company.Route.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.Route.PL.Controllers
{
    public class DepartmentController : Controller
    {
        // make object from this class 
        private readonly IDepartmentRespstory _departmentRepository;

        // ask CLR to create object from DepartmentRepsoitpooyrt
        public DepartmentController(IDepartmentRespstory departmentRepository)
        {
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
        public IActionResult Create(Department model)
        {
            if (ModelState.IsValid)
            {
                var create = _departmentRepository.Add(model);
                if (create > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        public IActionResult Details(int? id)
        {
            // Bad Request
            if (id is null) return BadRequest(); // 400
            var result = _departmentRepository.GetById(id.Value);
            // Not Found
            if(result is null)
            {
                return NotFound(); // 404

            }
            return View(result);
        }
    }

}
