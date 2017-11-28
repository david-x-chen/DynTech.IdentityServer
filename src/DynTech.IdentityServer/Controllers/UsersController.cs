using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynTech.IdentityServer.Models;
using DynTech.IdentityServer.Models.UserViewModels;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DynTech.IdentityServer.Controllers
{
    /// <summary>
    /// Users controller.
    /// </summary>
    [Authorize("View Users")]
    [Route("[controller]/[action]")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IClientStore _clients;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DynTech.IdentityServer.Controllers.UsersController"/> class.
        /// </summary>
        /// <param name="userManager">User manager.</param>
        /// <param name="roleManager">role manager.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="clients">clients.</param>
        public UsersController(UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               ILogger<UsersController> logger,
                               IClientStore clients)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _clients = clients;
        }

        /// <summary>
        /// Index this instance.
        /// </summary>
        /// <returns>The index.</returns>
        [Authorize("View Users")]
        public async Task<IActionResult> Index()
        {
            var userList = new List<UserViewModel>();

            var userRole = await _roleManager.FindByNameAsync("SiteUser");
            if (userRole != null)
            {
                var users = await _userManager.GetUsersInRoleAsync(userRole.NormalizedName);
                foreach (var user in users)
                {
                    userList.Add(new UserViewModel()
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        ClientNames = GetClients(user).Result
                    });
                }
            }

            var model = new UsersViewModel()
            {
                Users = userList
            };

            return View(model);
        }

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <returns>The clients.</returns>
        /// <param name="user">User.</param>
        private async Task<List<string>> GetClients(ApplicationUser user)
        {
            var results = new List<string>();
            if (user.Clients != null)
            {
                foreach (var clientId in user.Clients)
                {
                    var client = await _clients.FindEnabledClientByIdAsync(clientId.ToString());
                    results.Add(client.ClientName);
                }
            }

            return results;
        }
    }
}
