using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Route.DAL.Models
{
    public class Employee : BaseEntity
    {

        // All Client Side Validation must be in anthor class called View Model*

     
        public string Address { get; set; }

        
        public int? Age { get; set; }
      
        public decimal Salary { get; set; }

       
        public string Email { get; set; }

        public int PhoneNumber { get; set; }

        public bool isActive { get; set; }

        public bool isDeleted { get; set; }

        public DateTime HiringDate { get; set; }

        public int? WorkForId { get; set; } // FK

        // Navigational Property
        // EF Core by default : Didn't Loading The Navigitional Property
        public Department? WorkFor {  get; set; }



    }
}
