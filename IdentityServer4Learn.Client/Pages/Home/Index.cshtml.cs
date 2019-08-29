using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer4Learn.Client.Pages.Home
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public IEnumerable<Claim> UserClaims;
        public IDictionary<string, string> Properties;

        public async Task OnGet()
        {
            UserClaims = User.Claims;
            Properties = (await HttpContext.AuthenticateAsync()).Properties.Items;
        }
    }
}