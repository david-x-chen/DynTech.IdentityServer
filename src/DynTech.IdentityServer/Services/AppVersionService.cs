using System.Reflection;

namespace DynTech.IdentityServer.Services
{
    public class AppVersionService : IAppVersionService
    {
        public string Version => 
            Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    } 
}