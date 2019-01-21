namespace Tests
{
	using System.Security.Claims;
	using Microsoft.AspNetCore.Identity.MongoDB;
	using NUnit.Framework;

	[TestFixture]
	public class IdentityRoleClaimTests : AssertionHelper
	{
		[Test]
		public void Create_FromClaim_SetsTypeAndValue()
		{
			var claim = new Claim("type", "value");

			var roleClaim = new IdentityRoleClaim(claim);

			Expect(roleClaim.Type, Is.EqualTo("type"));
			Expect(roleClaim.Value, Is.EqualTo("value"));
		}

		[Test]
		public void ToSecurityClaim_SetsTypeAndValue()
		{
			var roleClaim = new IdentityRoleClaim {Type = "t", Value = "v"};

			var claim = roleClaim.ToSecurityClaim();

			Expect(claim.Type, Is.EqualTo("t"));
			Expect(claim.Value, Is.EqualTo("v"));
		}
	}
}