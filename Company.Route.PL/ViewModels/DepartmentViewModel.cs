using Company.Route.DAL.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Company.Route.PL.ViewModels
{
    public class DepartmentViewModel : BaseEntity
    {

        [Required(ErrorMessage = "Code is required *")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is required *")]
        public string? Name { get; set; }

        [DisplayName("Date Of Creation")]
        public DateTime? DateOfCreation { get; set; } = DateTime.Now;

        // One To Many Collection Or List Or Sequence...
        public ICollection<Employee>? Employees { get; set; }


    }
}
