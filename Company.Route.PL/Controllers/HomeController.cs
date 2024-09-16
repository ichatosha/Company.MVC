using Company.Route.PL.Models;
using Company.Route.PL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace Company.Route.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScopedService     _scope01;
        private readonly IScopedService     _scope02;
        private readonly ITransientService _transient01;
        private readonly ITransientService _transient02;
        private readonly ISingletonService _singleton01;
        private readonly ISingletonService _singleton02;

        public HomeController(ILogger<HomeController> logger,
            IScopedService scope01,IScopedService scope02,
            ITransientService transient01, ITransientService transient02,
            ISingletonService singleton01, ISingletonService singleton02
            )
        {
            _logger = logger;
            _scope01 = scope01;
            _scope02 = scope02;
            _transient01 = transient01;
            _transient02 = transient02;
            _singleton01 = singleton01;
            _singleton02 = singleton02;
        }



        // Home/TestLifeTime
        public string TestLifeTime()
        {
            StringBuilder builder = new StringBuilder();

            // Change if Refresh with the same values
            builder.Append($"scope01 :: {_scope01.GetGuid()}/n");           // 308a91fc-faef-4bed-a56e-c1587556a8a9
            builder.Append($"scope01 :: {_scope02.GetGuid()}/n/n");         // 308a91fc-faef-4bed-a56e-c1587556a8a9

            //Change if Refresh with diffrent values
            builder.Append($"transient01 :: {_transient01.GetGuid()}/n");   // 645c8aa7-0003-428f-a5b2-dc59ff15db2c 
            builder.Append($"transient02 :: {_transient02.GetGuid()}/n/n"); // 24e9da2e-46e5-428f-8c57-402098f3e961    

            // Fixed if Refresh
            builder.Append($"singleton01 :: {_singleton01.GetGuid()}/n");   // ab672198-da60-4450-a6b6-d2ddb099560a  
            builder.Append($"singleton02 :: {_singleton02.GetGuid()}/n/n"); // ab672198-da60-4450-a6b6-d2ddb099560a

            return builder.ToString();
        }







        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
