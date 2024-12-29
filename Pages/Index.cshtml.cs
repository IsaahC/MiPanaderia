using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Panaderia.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        // Validar si el usuario está autenticado
        if (HttpContext.Session.GetString("UserAuthenticated") != "true")
        {
            // Si no está autenticado, redirigir a la página de Login
            return RedirectToPage("/Login");
        }

        // Código adicional para manejar la lógica de la página
        return Page();
    }
}
