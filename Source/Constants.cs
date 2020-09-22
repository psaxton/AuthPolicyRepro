using Microsoft.IdentityModel.Tokens;
using System;

namespace AuthPolicyRepro
{
	public static class Constants
	{
		public const string CustomPrincipalScheme = nameof(CustomPrincipalScheme);
		public const string ExplicitSchemePolicy = nameof(ExplicitSchemePolicy);
		public const string ImplicitSchemePolicy = nameof(ImplicitSchemePolicy);
		public const string TestAudience = nameof(TestAudience);
		public const string TestIssuer = "https://" + nameof(TestIssuer);
		public const string TestScope = nameof(TestScope);

		public static readonly SecurityKey SecurityKey = new SymmetricSecurityKey(Convert.FromBase64String(@"
			XEsV+JSz9825Ija32algpDPuk16G2QegKw0pxjxMrkny0ij5QqyQQ0cnP
			gUtoJquF0xpGYIXe4JDdMyHiTCRbHPtxQaBZmGHXk8WXpsBc4tze67v45
			XH2lalts7gCLoXltGSQNqTLbHq4rsyARznQQFAEw6CoYneJuqw27CTplW
			U/8jJbzOw3039BQDalNk6vnTNn/WslfVHtBrw41IAVztlngFUHdHNjzPA
			hJyJA5ERdhw8xgdg6GrTgbsZwPz7/a3EdPbX5aCi/w/B6xBYvrCI3m01x
			JE0BbWXgZwWtM83xYfuxY+bOpB8mcb3nvxZesIKkOGPxTgcj25AP2QEiw
			=="));
	}
}
