using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class RoleBasedInfoModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public bool isAdmin { get; private set; }
    public bool isUser { get; private set; }

    public RoleBasedInfoModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task OnGetAsync()
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            isAdmin = roles.Contains("Admin");
            isUser = roles.Contains("User");
        }
    }
}