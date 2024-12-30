using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

public class LoginModel : PageModel
{
    [BindProperty]
    public string NombreUsuario { get; set; } = string.Empty;

    [BindProperty]
    public string Contrasena { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(NombreUsuario) || string.IsNullOrWhiteSpace(Contrasena))
        {
            ErrorMessage = "Por favor, ingrese el usuario y la contraseña.";
            return Page();
        }

        try
        {
             string connectionString = "Server=DESKTOP-LOAE6OF;Database=Panes;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Contrasena FROM Usuarios WHERE NombreUsuario = @NombreUsuario";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@NombreUsuario", NombreUsuario);

                    var result = command.ExecuteScalar();
                    if (result != null && Contrasena == result.ToString())
                    {
                        HttpContext.Session.SetString("UserAuthenticated", "true");
                        return RedirectToPage("/Productos/Index");
                    }
                }

                ErrorMessage = "Usuario o contraseña incorrectos.";
                return Page();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Error al iniciar sesión: " + ex.Message;
            return Page();
        }
    }
}
