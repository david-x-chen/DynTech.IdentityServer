using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DynTech.IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using IdentityServer4.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace DynTech.IdentityServer.Controllers
{
    /// <summary>
    /// Home controller.
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DynTech.IdentityServer.Controllers.HomeController"/> class.
        /// </summary>
        /// <param name="interaction">Interaction.</param>
        /// <param name="httpContextAccessor">Http context accessor.</param>
        public HomeController(IIdentityServerInteractionService interaction,
            IHttpContextAccessor httpContextAccessor)
        {
            _interaction = interaction;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Index this instance.
        /// </summary>
        /// <returns>The index.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// About this instance.
        /// </summary>
        /// <returns>The about.</returns>
        [Route("Home/About")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        /// <summary>
        /// Contact this instance.
        /// </summary>
        /// <returns>The contact.</returns>
        [Route("Home/Contact")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Error the specified errorId.
        /// </summary>
        /// <returns>The error.</returns>
        /// <param name="errorId">Error identifier.</param>
        [Route("Home/Error")]
        public async Task<IActionResult> Error([FromQuery]string errorId)
        {
            var errMsg = new ErrorMessage();
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                errMsg = message;
            }

            if (string.IsNullOrEmpty(errorId))
            {
                var errorState = TempData["ModelState"] as List<string>;
                if (errorState != null)
                {
                    foreach (var err in errorState)
                    {
                        System.Console.WriteLine(err);
                        errMsg.Error += err;
                    }
                }
            }

            var context = _httpContextAccessor.HttpContext;
            var ex = context.Features.Get<IExceptionHandlerFeature>();
            if (ex != null)
            {
                errMsg.Error += $"<br /><h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace }";
            }

            vm.Error = errMsg;

            return View("Error", vm);
        }
    }
}
