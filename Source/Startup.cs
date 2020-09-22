using AuthPolicyRepro.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

using static AuthPolicyRepro.Constants;

namespace AuthPolicyRepro
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(CustomPrincipalScheme)
				.AddJwtBearer(CustomPrincipalScheme, options =>
				{
					options.Events = new JwtBearerEvents
					{
						OnTokenValidated = context =>
						{
							context.Principal = new CustomPrincipal(context.Principal);
							return Task.CompletedTask;
						}
					};
					options.IncludeErrorDetails = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						IssuerSigningKey = Constants.SecurityKey,
						ValidIssuer = TestIssuer,
						ValidAudience = TestAudience,
					};
				});

			services.AddAuthorization(options =>
			{
				options.AddPolicy(ExplicitSchemePolicy, policyBuilder =>
					policyBuilder
						.AddAuthenticationSchemes(CustomPrincipalScheme)
						.RequireClaim("scope", TestScope)
						.RequireAssertion(context => context.User is CustomPrincipal)
				);

				options.AddPolicy(ImplicitSchemePolicy, policyBuilder =>
					policyBuilder
						.RequireClaim("scope", TestScope)
						.RequireAssertion(context => context.User is CustomPrincipal)
				);
			});

			services.AddMvc();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseDeveloperExceptionPage();
			app.UseAuthentication();
			app.UseMvc();
		}
	}
}
