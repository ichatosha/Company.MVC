using AutoMapper;
using Company.Route.DAL.Models;
using Company.Route.PL.ViewModels;

namespace Company.Route.PL.Mapping
{ 
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            // mapping from DepartmentViewModel to Department And Opposite
            CreateMap<DepartmentViewModel,Department>().ReverseMap();
        }
    }
}
