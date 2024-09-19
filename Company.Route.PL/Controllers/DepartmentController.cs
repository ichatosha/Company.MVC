using AutoMapper;
using Company.Route.BLL.Interfaces;
using Company.Route.BLL.Repsitories;
using Company.Route.DAL.Data.Contexts;
using Company.Route.DAL.Models;
using Company.Route.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Company.Route.PL.Controllers
{
    public class DepartmentController : Controller
    {
        // make object from this interface 
        private readonly IDepartmentRespstory _departmentRepository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        
        // ask CLR to create object from DepartmentRepository
        public DepartmentController(AppDbContext context , IDepartmentRespstory departmentRepository, IMapper mapper)
        {
            _context = context;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }







        #region Get All
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var allDepartments = Enumerable.Empty<Department>();
            var AllDepartments = await _departmentRepository.GetAllAsync();

            //Auto Mapping
            var result = _mapper.Map<IEnumerable<DepartmentViewModel>>(AllDepartments);

            return View(result);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        // decline any outside token can edit in web Like : Postman 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DateOfCreation == null)
                {
                    model.DateOfCreation = DateTime.Now;
                }
                var result = _mapper.Map<Department>(model);
                var create = await _departmentRepository.AddAsync(result);
                if (create > 0)
                {
                    TempData["Message"] = "Department is Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Message"] = "Department is Not Created Successfully";
                }
            }
            return View(model);
        }

        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id , string viewName = "Details")
        {
            // Bad Request
            if (id is null) return BadRequest(); // 400
            var result = await _departmentRepository.GetByIdAsync(id.Value);
            // Not Found
            if(result is null)
            {
                return NotFound(); // 404
            }
            var ViewModel = _mapper.Map<DepartmentViewModel>(result);
            return View(viewName ,ViewModel);
        }
        #endregion

        #region Update
        // Update
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
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
            return await Details(id , "Update");
        }

        [HttpPost]
        // decline any outside token can edit in web Like : Postman 
        [ValidateAntiForgeryToken]
        // [FromRoute] >> to take the id from segment only not from form
        public async Task<IActionResult> Update([FromRoute] int? id , DepartmentViewModel UpdatedDepartment)
        {
            if (id != UpdatedDepartment.Id) return BadRequest(); // 400

            if(ModelState.IsValid)
            {
                var ViewModel = _mapper.Map<Department>(UpdatedDepartment);
                var result = await _departmentRepository.UpdateAsync(ViewModel);
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

        #endregion 

        #region Delete
        // Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id is null) { return BadRequest(); } // 400

            //var result = _departmentRepository.GetById(id.Value);

            //if (result is null)
            //{
            //    return NotFound(); // 404
            //}
            //return View(result);

            // lessw the repetition of code
            return await Details(id, "Delete");

        }

        

        [HttpPost]
        //decline any outside token can edit in web Like : Postman 
        [ValidateAntiForgeryToken]
        // [FromRoute] >> to take the id from segment only not from form
        public async Task<IActionResult> Delete([FromRoute] int? id , DepartmentViewModel deletedDepartment)
        {
            if (id is null) return BadRequest(); // 400

            if (id != deletedDepartment.Id) return BadRequest(); // 400

            if (ModelState.IsValid)
            {
                var ViewModel = _mapper.Map<Department>(deletedDepartment);
                var result = await _departmentRepository.DeleteAsync(ViewModel);
                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            // else : stay in view
            return View(deletedDepartment);

        }
        #endregion
    }

}
