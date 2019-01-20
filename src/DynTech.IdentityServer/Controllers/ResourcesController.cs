
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynTech.IdentityServer.Models.ResourceModels;
using IdentityServer4.MongoDB.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DynTech.IdentityServer.Controllers
{
    [Authorize("View Resources")]
    [Route("[controller]/[action]")]
    public class ResourcesController: Controller
    {
        private readonly IConfigurationDbContext _context;        
        private readonly ILogger _logger;

        public ResourcesController(IConfigurationDbContext context,
                                ILogger<ResourcesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> IdResList()
        {
            var resList = _context.IdentityResources.ToList()
                                     .Select(rm => {
                                          return new IdentityResourceViewModel
                                          {
                                              Id = rm.Id.ToString(),
                                              Name = rm.Name,
                                              Description = rm.Description,
                                              DisplayName = rm.DisplayName
                                          };
                                      }).ToList();

            var model = new IdentityResourcesViewModel()
            {
                IdentityResources = resList
            };

            await Task.Delay(0);

            return View(model);
        }

        public async Task<IActionResult> ApiResList()
        {
            _logger.LogInformation("Load api resources...");

            var resList = _context.ApiResources.ToList()
                                     .Select(rm => {
                                          return new IdentityResourceViewModel
                                          {
                                              Id = rm.Id.ToString(),
                                              Name = rm.Name,
                                              Description = rm.Description,
                                              DisplayName = rm.DisplayName
                                          };
                                      }).ToList();

            var model = new IdentityResourcesViewModel()
            {
                IdentityResources = resList
            };

            await Task.Delay(0);

            return View(model);
        }
    }
}