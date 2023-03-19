using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ServiceManagment
{
	public static class ClaimParticipalExtensions
	{
		public static string GetUserId(this ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.NameIdentifier).Value;
		}
	}
}
