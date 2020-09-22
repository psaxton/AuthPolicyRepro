using System.Security.Claims;

namespace AuthPolicyRepro.Authentication
{
	public class CustomPrincipal : ClaimsPrincipal
	{
		public CustomPrincipal(ClaimsPrincipal principal) : base(principal) { }
	}
}
