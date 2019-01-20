using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynTech.IdentityServer.Models;
using DynTech.IdentityServer.Models.ClientViewModels;
using IdentityServer4.Models;
using IdentityServer4.MongoDB.Interfaces;
using IdentityServer4.MongoDB.Mappers;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DynTech.IdentityServer.Controllers
{
    /// <summary>
    /// Clients controller.
    /// </summary>
    [Authorize("View Clients")]
    [Route("[controller]/[action]")]
    public class ClientsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfigurationDbContext _context;
        private readonly IClientStore _clients;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DynTech.IdentityServer.Controllers.ClientsController"/> class.
        /// </summary>
        /// <param name="clients">Clients.</param>
        /// <param name="context">Context.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="userManager">user Manager.</param>
        public ClientsController(IClientStore clients,
                                 IConfigurationDbContext context,
                                 ILogger<ClientsController> logger,
                                 UserManager<ApplicationUser> userManager)
        {
            _clients = clients;
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        /// <summary>
        /// Index this instance.
        /// </summary>
        /// <returns>The index.</returns>
        public async Task<IActionResult> Index()
        {
            var clientList = new List<ClientViewModel>();
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser != null && currentUser.Clients != null)
            {
                clientList = _context.Clients.Where(c => currentUser.Clients.Contains(c.ClientId)).ToList()
                                     .Select(cm =>
                                      {
                                          var client = new ClientViewModel
                                          {
                                              ClientId = cm.ClientId,
                                              ClientName = cm.ClientName
                                          };
                                          return client;
                                      }).ToList();
            }

            var model = new ClientsViewModel()
            {
                Clients = clientList
            };

            return View(model);
        }

        /// <summary>
        /// Detail the specified clientId.
        /// </summary>
        /// <returns>The detail.</returns>
        /// <param name="id">Client identifier.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> Detail(string id = "-")
        {
            var model = new ClientViewModel();

            if (id != "-")
            {
                var client = await _clients.FindClientByIdAsync(id);
                model = new ClientViewModel
                {
                    ClientId = client.ClientId,
                    ClientName = client.ClientName,
                    ClientSecret = client.ClientSecrets.FirstOrDefault().Value,
                    AllowedGrantType = client.AllowedGrantTypes.FirstOrDefault(),
                    RedirectUris = client.RedirectUris.ToList(),
                    RedirectUrisString = string.Join(";", client.RedirectUris),
                    PostLogoutRedirectUris = client.PostLogoutRedirectUris.ToList(),
                    PostLogoutRedirectUrisString = string.Join(";", client.PostLogoutRedirectUris),
                    AllowedCorsOrigins = client.AllowedCorsOrigins.ToList(),
                    AllowedCorsOriginsString = string.Join(";", client.AllowedCorsOrigins)
                };
            }

            return View(model);
        }

        /// <summary>
        /// Detail the specified client.
        /// </summary>
        /// <returns>The detail.</returns>
        /// <param name="model">Client model.</param>
        [HttpPost("{0}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(ClientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = _context.Clients.Where(c => c.ClientId == model.ClientId).FirstOrDefault();
                if (client == null)
                {
                    var clientModel = new Client
                    {
                        ClientId = model.ClientId,
                        ClientName = model.ClientName,
                        AllowedGrantTypes = GetGrantType(model.AllowedGrantType),
                        AllowAccessTokensViaBrowser = true,
                        RedirectUris = model.RedirectUrisString.Split(";").ToList(),
                        PostLogoutRedirectUris = model.PostLogoutRedirectUrisString.Split(";").ToList(),
                        AllowedCorsOrigins = model.AllowedCorsOriginsString.Split(";").ToList(),
                        AllowOfflineAccess = true,
                        AllowedScopes = model.AllowedScopes
                    };

                    clientModel.ClientSecrets.Add(new Secret(model.ClientSecret.Sha256()));

                    await _context.AddClient(clientModel.ToEntity());
                }
                else
                {
                    var clientModel = client.ToModel();
                    var id = client.Id;

                    clientModel.ClientName = model.ClientName;
                    clientModel.AllowedGrantTypes = GetGrantType(model.AllowedGrantType);
                    clientModel.RedirectUris = model.RedirectUrisString.Split(";").ToList();
                    clientModel.PostLogoutRedirectUris = model.PostLogoutRedirectUrisString.Split(";").ToList();
                    clientModel.AllowedCorsOrigins = model.AllowedCorsOriginsString.Split(";").ToList();

                    client = clientModel.ToEntity();
                    client.Id = id;

                    await _context.AddClient(client);
                }

                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                if (currentUser != null)
                {
                    if (currentUser.Clients == null)
                    {
                        currentUser.Clients = new List<string>();
                    }

                    currentUser.AddClient(model.ClientId);

                    await _userManager.UpdateAsync(currentUser);
                }
            }
            else
            {
                var errorStates = new List<string>();
                var modelError = ModelState.Values.Where(m => m.Errors.Count > 0)
                    .SelectMany(e => e.Errors).ToList();
                foreach (var err in modelError)
                {
                    System.Console.WriteLine(err.ErrorMessage);
                    errorStates.Add($"<br /><h1>Error: {err.ErrorMessage}</h1>");
                }

                TempData["ModelState"] = errorStates;

                return RedirectToAction("Error", "Home");
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Gets the type of the grant.
        /// </summary>
        /// <returns>The grant type.</returns>
        /// <param name="grantTypeName">Grant type name.</param>
        private static ICollection<string> GetGrantType(string grantTypeName)
        {
            switch (grantTypeName)
            {
                case "ClientCredentials":
                    return GrantTypes.ClientCredentials;
                case "Code":
                    return GrantTypes.Code;
                case "CodeAndClientCredentials":
                    return GrantTypes.CodeAndClientCredentials;
                case "Hybrid":
                    return GrantTypes.Hybrid;
                case "HybridAndClientCredentials":
                    return GrantTypes.HybridAndClientCredentials;
                case "Implicit":
                    return GrantTypes.Implicit;
                case "ImplicitAndClientCredentials":
                    return GrantTypes.ImplicitAndClientCredentials;
                case "ResourceOwnerPassword":
                    return GrantTypes.ResourceOwnerPassword;
                case "ResourceOwnerPasswordAndClientCredentials":
                    return GrantTypes.ResourceOwnerPasswordAndClientCredentials;
                default:
                    return GrantTypes.CodeAndClientCredentials;
            }
        }
    }
}
