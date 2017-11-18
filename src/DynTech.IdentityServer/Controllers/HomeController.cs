using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DynTech.IdentityServer.Models;

namespace DynTech.IdentityServer.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interaction"></param>
        public HomeController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [Route("Home/About")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Route("Home/Contact")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Route("Home/Error")]
        public async Task<IActionResult> Error([FromQuery]string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;
            }

            return View("Error", vm);
        }
    }
}
