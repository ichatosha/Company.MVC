using AutoMapper;
using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repsitories;
using Company.Route.DAL.Data.Contexts;
using Company.Route.DAL.Models;
using Company.Route.PL.Helpers;
using Company.Route.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Reflection.Metadata;

namespace Company.Route.PL.Controllers
{
	[Authorize]
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
        public async Task<IActionResult> Index(string WordSearch)
        {

            var AllEmployees = Enumerable.Empty<Employee>();
            //var employeeViewModels = new Collection<EmployeeViewModel>();
            if (string.IsNullOrEmpty(WordSearch))
            {
                 AllEmployees = await _employeeRepository.GetAllAsync();
            }
            else
            {
                AllEmployees = await _employeeRepository.GetByNameAsync(WordSearch);
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

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    var  AllEmployees = _employeeRepository.ToList();
        //    return View(AllEmployees);
        //}
        #endregion


        #region Create
        // CREATE
        [HttpGet]
        public async Task<IActionResult> Create()
        { 
            var departments = await _departmentRespstory.GetAllAsync(); // Extra Information 
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
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.imageName = model.Image != null ? DocumentSettings.UploadFile(model.Image, "images") : null;
                //model.imageName = DocumentSettings.UploadFile(model.Image,"images");
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
                var result = await _employeeRepository.AddAsync(employee);
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
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {

            if (id is null) return BadRequest(); //400
            var result = await _employeeRepository.GetByIdAsync(id.Value);
            if (result == null) return NotFound(); //404
            // Convert From Employee To EmployeeViewModel
            var employee = _mapper.Map<EmployeeViewModel>(result);

            return View(ViewName, employee);
        }

        #endregion


        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            var departments = await _departmentRespstory.GetAllAsync(); // Extra Information 
            //EF Core by default : Don't Loading The Navigitional Property
            // Views Diact :
            // 2. ViewData
            ViewData["Departments"] = departments;
            return await Details(id, "Update");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(int? id, EmployeeViewModel model)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {

                if (model.imageName != null)
                {
                    DocumentSettings.DeleteFile(model.imageName, "images");
                }
                //model.imageName = DocumentSettings.UploadFile(model.Image, "images");
                model.imageName = model.Image != null ? DocumentSettings.UploadFile(model.Image, "images") : null;
                // Auto Mapping
                var employee = _mapper.Map<Employee>(model);
                var result = await _employeeRepository.UpdateAsync(employee);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            // else : stay in model
            return View(model);

        }

        public IActionResult Update()
        {
            var Employees = _context.Employees.ToList();
            return View(Employees);
        }

        #endregion


        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {

            return await Details(id, "Delete");
            
        }


        [HttpPost]
        //decline any outside token can edit in web Like : Postman 
        [ValidateAntiForgeryToken]
        // [FromRoute] >> to take the id from segment only not from form
        public async Task<IActionResult> Delete([FromRoute] int? id , EmployeeViewModel DeletedEmployee)
        {
            if (id is null) return BadRequest();

            if (ModelState.IsValid)
            {
                // Auto Mapper
                var DeleteEmployee = _mapper.Map<Employee>(DeletedEmployee);
                var result = await _employeeRepository.DeleteAsync(DeleteEmployee);
                if (result > 0)
                {
                    DocumentSettings.DeleteFile(DeletedEmployee.imageName, "images");
                    return RedirectToAction("Index");
                }
            }
            // else : stay in model
            return View(DeletedEmployee);
        }
        #endregion


        
    }
}
