using DynTech.IdentityServer.Services;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;

namespace DynTech.IdentityServer.Test
{
    public class MessageServicesTest
    {
        [Fact]
        public void SendEmail()
        {
            var service = new AuthMessageSender();
            service.SendEmailAsync("", "", "").Should().NotBeNull();
        }
    }
}
