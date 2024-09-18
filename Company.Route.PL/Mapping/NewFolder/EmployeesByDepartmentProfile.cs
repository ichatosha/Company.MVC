using AutoMapper;
using Company.Route.DAL.Models;
using Company.Route.PL.ViewModels;

namespace Company.Route.PL.Mapping.NewFolder
{
    public class EmployeesByDepartmentProfile : Profile
    {

        public EmployeesByDepartmentProfile()
        {
            CreateMap<EmployeeViewModel,Employee>().ReverseMap();
        }

    }
}
