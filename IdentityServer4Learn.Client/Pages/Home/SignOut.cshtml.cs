using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer4Learn.Client.Pages.Home
{
    public class SignOutModel : PageModel
    {
        public IActionResult OnGet()
        {
           return SignOut("Cookies", "oidc");
        }
    }
}