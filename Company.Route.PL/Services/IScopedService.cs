using Company.Route.PL.Services;

namespace Company.Route.PL.Services
{
    public interface IScopedService
    {
        public Guid Guid { get; set; }

        string GetGuid();

    }
}
