using AutoMapper;
using Company.Route.DAL.Models;
using Company.Route.PL.ViewModels;

namespace Company.Route.PL.Mapping
{
    public class EmployeeProfile : Profile
    {

        public EmployeeProfile()
        {
            // mapping from EmployeeViewModel to Employee And Opposite
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            
;        }
    }
}
