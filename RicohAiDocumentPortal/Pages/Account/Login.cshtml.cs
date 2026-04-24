using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RicohAiDocumentPortal.Pages.Account;

public class LoginModel : PageModel
{
    private const string AuthenticatedKey = "IsAuthenticated";
    private const string UserEmailKey = "UserEmail";

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString(AuthenticatedKey) == "true")
        {
            return Redirect("/Home");
        }

        return Page();
    }

    public IActionResult OnPostSession([FromBody] LoginSessionRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Email))
        {
            return new BadRequestObjectResult(new
            {
                success = false,
                message = "Email is required."
            });
        }

        HttpContext.Session.SetString(AuthenticatedKey, "true");
        HttpContext.Session.SetString(UserEmailKey, request.Email.Trim());

        return new JsonResult(new
        {
            success = true,
            redirectUrl = "/Home"
        });
    }

    public IActionResult OnPostLogout()
    {
        HttpContext.Session.Clear();

        return new JsonResult(new
        {
            success = true,
            redirectUrl = "/Account/Login"
        });
    }

    public class LoginSessionRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }
}
