using AuthPolicyRepro.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static AuthPolicyRepro.Constants;

namespace AuthPolicyRepro.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		[HttpGet("explicit"), Authorize(Policy = ExplicitSchemePolicy)]
		public ActionResult<bool> ExplicitGet()
		{
			return User is CustomPrincipal;
		}

		[HttpGet("implicit"), Authorize(Policy = ImplicitSchemePolicy)]
		public ActionResult<bool> ImplicitGet()
		{
			return User is CustomPrincipal;
		}
	}
}
