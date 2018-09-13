
using System.Collections.Generic;

namespace DynTech.IdentityServer.Models.ResourceModels
{
    public class IdentityResourcesViewModel
    {
        public List<IdentityResourceViewModel> IdentityResources { get; set; }
    }

    public class IdentityResourceViewModel
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string Description { get; internal set; }
        public string DisplayName { get; internal set; }
    }
}