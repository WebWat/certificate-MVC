using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Areas.Identity.Pages.Account;

[Authorize]
public class AccessDeniedModel : PageModel
{
    public void OnGet(string? returnUrl = null)
    {
    }
}
