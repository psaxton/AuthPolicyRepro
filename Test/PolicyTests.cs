using AuthPolicyRepro;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

using static AuthPolicyRepro.Constants;

namespace AuthPolicyReproTests
{
	[TestClass]
	public class PolicyTests
	{
		private WebApplicationFactory<Startup> TestFactory;
		private string BearerToken;

		[TestInitialize]
		public void Setup()
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim("scope", TestScope)
				}),
				Expires = DateTime.UtcNow.AddSeconds(30),
				Issuer = TestIssuer,
				Audience = TestAudience,
				SigningCredentials = new SigningCredentials(Constants.SecurityKey, SecurityAlgorithms.HmacSha256),
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			BearerToken = tokenHandler.WriteToken(token);

			TestFactory = new WebApplicationFactory<Startup>();
		}

		[TestCleanup]
		public void TearDown()
		{
			BearerToken = null;

			if (TestFactory != null)
			{
				TestFactory.Dispose();
				TestFactory = null;
			}
		}

		[TestMethod]
		public async Task TestExplicitSchemePolicy()
		{
			using (var client = TestFactory.CreateClient())
			{
				client.DefaultRequestHeaders.Add("Authorization", $"Bearer {BearerToken}");
				var result = await client.GetAsync("api/values/explicit");
				var response = await result.Content.ReadAsStringAsync();

				Assert.IsTrue(result.IsSuccessStatusCode);
				Assert.AreEqual("true", response);
			}
		}

		[TestMethod]
		public async Task TestImplicitSchemePolicy()
		{
			using (var client = TestFactory.CreateClient())
			{
				client.DefaultRequestHeaders.Add("Authorization", $"Bearer {BearerToken}");
				var result = await client.GetAsync("api/values/implicit");
				var response = await result.Content.ReadAsStringAsync();

				Assert.IsTrue(result.IsSuccessStatusCode);
				Assert.AreEqual("true", response);
			}
		}
	}
}
