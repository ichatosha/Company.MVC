using Company.Route.DAL.Models;
using System.ComponentModel.DataAnnotations;
using Company.Route.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Company.Route.PL.ViewModels
{
    public class EmployeeViewModel : BaseEntity
    {
        // All Client Side Validation must be in anthor class called View Model*

        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}",
            ErrorMessage = ("Address Must be Like 123-Street-City-Country"))]
        public string Address { get; set; }

        [Range(23, 59, ErrorMessage = "Age must be between 23 and 59")]
        public int? Age { get; set; }
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Salary is required !")]
        public decimal Salary { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //[Phone]
        [RegularExpression(@"^(?:\+20|0)?1[0125]\d{8}$", ErrorMessage = "phone number must be like this +2001012345678")]
        public int PhoneNumber { get; set; }

        public bool isActive { get; set; }

        public bool isDeleted { get; set; }

        public DateTime HiringDate { get; set; }

        [ForeignKey("Department")]
        public int? WorkForId { get; set; } // FK

        // Navigational Property
        // EF Core by default : Didn't Loading The Navigitional Property
        public Department? WorkFor { get; set; }

    }
}
