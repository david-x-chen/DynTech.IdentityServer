namespace IntegrationTests
{
	using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.MongoDB;
	using Microsoft.Extensions.DependencyInjection;
    using Mongo2Go;
    using MongoDB.Driver;
    using Moq;
    using NUnit.Framework;

	[SetUpFixture]
	public class UserIntegrationTestsBase: AssertionHelper
	{
    	internal static MongoDbRunner _runner;
		protected IMongoDatabase Database;
		protected IMongoCollection<IdentityUser> Users;
		protected IMongoCollection<IdentityRole> Roles;

		// note: for now we'll have interfaces to both the new and old apis for MongoDB, that way we don't have to update all the tests at once and risk introducing bugs
		protected IMongoDatabase DatabaseNewApi;
		protected IServiceProvider ServiceProvider;
        private readonly string _TestingConnectionString = "";//$"mongodb://localhost:27017/{IdentityTesting}";
		private const string IdentityTesting = "identity-testing";

        public UserIntegrationTestsBase()
        {
			_runner = MongoDbRunner.Start(singleNodeReplSet: false);
			_TestingConnectionString = _runner.ConnectionString + IdentityTesting;

        }

        [OneTimeSetUp]
		public void BeforeEachTest()
		{
			Console.WriteLine($"in memory connection: {_runner.ConnectionString}");

			var client = new MongoClient(_runner.ConnectionString);

			// todo move away from GetServer which could be deprecated at some point
			Database = client.GetDatabase(IdentityTesting);
			Users = Database.GetCollection<IdentityUser>("users");
			Roles = Database.GetCollection<IdentityRole>("roles");

			DatabaseNewApi = client.GetDatabase(IdentityTesting);

			Database.DropCollection("users");
			Database.DropCollection("roles");

			ServiceProvider = CreateServiceProvider<IdentityUser, IdentityRole>();
		}

		[OneTimeTearDown]
		public void AfterTest()
		{

		}

		protected UserManager<IdentityUser> GetUserManager()
			=> ServiceProvider.GetService<UserManager<IdentityUser>>();

		protected RoleManager<IdentityRole> GetRoleManager()
			=> ServiceProvider.GetService<RoleManager<IdentityRole>>();

		protected IServiceProvider CreateServiceProvider<TUser, TRole>(Action<IdentityOptions> optionsProvider = null)
			where TUser : IdentityUser
			where TRole : IdentityRole
		{
			var services = new ServiceCollection();
			optionsProvider = optionsProvider ?? (options => { });
			services.AddIdentity<TUser, TRole>(optionsProvider)
				.AddDefaultTokenProviders()
				.RegisterMongoStores<TUser, TRole>(_TestingConnectionString);

			services.AddLogging();

			return services.BuildServiceProvider();
		}
	}

	public static class TaskExtensions
	{
		public static async Task WithTimeout(this Task task, TimeSpan timeout)
		{
			using (var cancellationTokenSource = new CancellationTokenSource())
			{

				var completedTask = await Task.WhenAny(task, Task.Delay(timeout, cancellationTokenSource.Token));
				if (completedTask == task)
				{
					cancellationTokenSource.Cancel();
					await task;
				}
				else
				{
					throw new TimeoutException("The operation has timed out.");
				}
			}
		}
	}
}