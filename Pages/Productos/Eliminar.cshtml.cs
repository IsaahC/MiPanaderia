using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Panaderia.Pages.Productos
{
    public class Eliminar : PageModel
    {
        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id_venta)
        {
            
        }

        public IActionResult OnPost(int id_venta)
        {
            try
            {
                eliminarProducto(id_venta);
                return RedirectToPage("/Productos/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = "No se pudo eliminar la venta: " + ex.Message;
                return Page();
            }
        }

        private void eliminarProducto(int id_venta)
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=Panes;Trusted_Connection=True;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "DELETE FROM Ventas WHERE Id_venta = @Id_venta";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id_venta", id_venta);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
