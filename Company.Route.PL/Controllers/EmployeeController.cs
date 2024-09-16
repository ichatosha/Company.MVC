using AutoMapper;
using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repsitories;
using Company.Route.DAL.Data.Contexts;
using Company.Route.DAL.Models;
using Company.Route.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Company.Route.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRespstory _departmentRespstory;
        private readonly IMapper _mapper;

        // ask CLR to create object from DepartmentRepository and AppDbContext
        public EmployeeController(
            AppDbContext context,
            IEmployeeRepository employeeRepository,
            IDepartmentRespstory departmentRespstory,
            IMapper mapper
            )
        {
            _context = context;
            _employeeRepository = employeeRepository;
            _departmentRespstory = departmentRespstory;
            _mapper = mapper;
        }

        #region Get All
        // [HttpGet] 
        // this function Work [httpPost] if WordSearch is not null
        // ............. Work [httpGet] if WordSearch is null
        // this function Works Get And Post together depends on paramter is null or not
        public IActionResult Index(string WordSearch)
        {

            var AllEmployees = Enumerable.Empty<Employee>();
            //var employeeViewModels = new Collection<EmployeeViewModel>();
            if (string.IsNullOrEmpty(WordSearch))
            {
                 AllEmployees =  _employeeRepository.GetAll();
            }
            else
            {
                AllEmployees = _employeeRepository.GetByName(WordSearch);
            }

            // Auto Mapping
            var result =  _mapper.Map<IEnumerable<EmployeeViewModel>>(AllEmployees);

            //employeeViewModels.Add(AllEmployees);
            //string Message = "Hello Our Client!";
            //// View's Dictionary : Extra Information Transfer Data from Action to View [One Way Only]
            //// 1. ViewData : Properity Inherited From Controller - Dictoinary
            //ViewData["MessageD"] = Message + " From ViewData";
            //// 2. ViewBag : Proerty Inherited From Controller - Dynamic
            //ViewBag.MessageB = Message + " From ViewBag";
            //// 3. TempData : Property Inherited From Controller - Dictionary
            //// Transfer Data From Request to Another (like : from Create to Index)
            //TempData["MessageT"] = Message + " From TempData";


            return View(result);
        }
        #endregion


        #region Create
        // CREATE
        [HttpGet]
        public IActionResult Create()
        { 
            var departments = _departmentRespstory.GetAll(); // Extra Information 
            //EF Core by default : Don't Loading The Navigitional Property
            // Views Diact :
            // 2. ViewData
            ViewData["Departments"] = departments;
            // 1. ViewBag
            // 3. TempData
            return View();
        }

        [HttpPost]
        // decline any outside token can edit in web Like : Postman 
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Casting From ViewModel To Model : 
                // EmployeeViewModel => Employee

                // Manual Mapping
                //Employee employee = new Employee()
                //{
                //    Age = model.Age,
                //    Address = model.Address,
                //    PhoneNumber = model.PhoneNumber,
                //    //......
                //}

                // Auto Mapping
                var employee =  _mapper.Map<Employee>(model);


                if (employee.DateOfCreation == null)
                {
                    employee.DateOfCreation = DateTime.Now;
                }
                var result = _employeeRepository.Add(employee);
                if (result > 0)
                {
                    TempData["Message"] = "Employee is Created Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "Employee is Not Created Successfully";
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
            var departments = _departmentRespstory.GetAll(); // Extra Information 
            //EF Core by default : Don't Loading The Navigitional Property
            // Views Diact :
            // 2. ViewData
            ViewData["Departments"] = departments;
            return Details(id, "Update");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Update(int? id, EmployeeViewModel UpdatedEmployee)
        {
            if (id != UpdatedEmployee.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                // Auto Mapping
                var employee = _mapper.Map<Employee>(UpdatedEmployee);
                var result = _employeeRepository.Update(employee);
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
        public IActionResult Delete([FromRoute] int? id , EmployeeViewModel DeletedEmployee)
        {
            if (id is null) return BadRequest();

            if (ModelState.IsValid)
            {
                // Auto Mapper
                var DeleteEmployee = _mapper.Map<Employee>(DeletedEmployee);
                var result = _employeeRepository.Delete(DeleteEmployee);
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
