namespace Tests
{
	using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Identity.MongoDB;
	using MongoDB.Bson;
	using NUnit.Framework;

	// todo low - validate all tests work
	[TestFixture]
	public class IdentityRoleTests : AssertionHelper
	{
		[Test]
		public void ToBsonDocument_IdAssigned_MapsToBsonObjectId()
		{
			var role = new IdentityRole();

			var document = role.ToBsonDocument();

			Expect(document["_id"], Is.TypeOf<BsonObjectId>());
		}

		[Test]
		public void Create_WithoutRoleName_HasIdAssigned()
		{
			var role = new IdentityRole();

			var parsed = role.Id.SafeParseObjectId();
			Expect(parsed, Is.Not.Null);
			Expect(parsed, Is.Not.EqualTo(ObjectId.Empty));
		}

		[Test]
		public void Create_WithRoleName_SetsName()
		{
			var name = "admin";

			var role = new IdentityRole(name);

			Expect(role.Name, Is.EqualTo(name));
		}

		[Test]
		public void Role_To_String()
		{
			var name = "admin";

			var role = new IdentityRole(name);

			Expect(role.ToString(), Is.EqualTo(name));
		}

		[Test]
		public void Create_WithRoleName_SetsId()
		{
			var role = new IdentityRole("admin");

			var parsed = role.Id.SafeParseObjectId();
			Expect(parsed, Is.Not.Null);
			Expect(parsed, Is.Not.EqualTo(ObjectId.Empty));
		}

		[Test]
		public void Add_New_Role_Claim()
		{
			var role = new IdentityRole("admin");
			role.AddClaim(new Claim ("edit", "edit"));

			var existingClaim = role.Claims.Any(c => c.Type == "edit");
			Expect(existingClaim, Is.True);
		}

		[Test]
		public void Add_New_Role_Claim_Then_Remove()
		{
			var role = new IdentityRole("admin");
			var claim = new Claim ("edit", "edit");
			role.AddClaim(claim);

			var existingClaim = role.Claims.Any(c => c.Type == claim.Type);
			Expect(existingClaim, Is.True);

			role.RemoveClaim(claim);

			existingClaim = role.Claims.Any(c => c.Type == claim.Type);
			Expect(existingClaim, Is.False);
		}

		[Test]
		public void Replace_Role_Claim()
		{
			var role = new IdentityRole("admin");
			var claim = new Claim ("edit", "edit");
			role.AddClaim(claim);

			var existingClaim = role.Claims.Any(c => c.Type == claim.Type);
			Expect(existingClaim, Is.True);

			var newClaim = new Claim("edit_all", "all");
			role.ReplaceClaim(claim, newClaim);

			existingClaim = role.Claims.Any(c => c.Type == newClaim.Type);
			Expect(existingClaim, Is.True);
		}
	}
}