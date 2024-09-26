using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Company.Route.PL.ViewModels
{
    public class RoleViewModel
    {
        [Key]
        public string? Id { get; set; }

        public string RoleName { get; set; }


    }
}
