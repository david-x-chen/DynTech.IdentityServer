using DynTech.IdentityServer.Services;
using Xunit;
using FluentAssertions;

namespace DynTech.IdentityServer.Test
{
    public class AppVersionServiceTest
    {
        [Fact]
        public void GetVersionNumber()
        {
            var service = new AppVersionService();
            service.Version.Should().StartWith("0.2");
        }
    }
}
