using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Panaderia.Pages.Productos
{
    public class Editar : PageModel
    {
        [BindProperty]
        public int Id_venta { get; set; }

        [BindProperty, Required(ErrorMessage = "Se requiere el producto")]
        public string Productos { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Ingrese la membresia del comprador")]
        public string Membresia { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Ingrese el nombre del encargado")]
        public string Encargado { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "Ingrese el total de la compra")]
        public decimal? Total { get; set; }

        [BindProperty, Required(ErrorMessage = "Ingrese el método de pago")]
        public string? Metodo { get; set; } = "";

        public string ErrorMessage { get; set; } = "";

        public void OnGet(int id_venta)
        {
            try
            {
                string connectionString = "Server=localhost\\SQLEXPRESS;Database=Panes;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM Ventas WHERE Id_venta=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id_venta);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Id_venta = reader.GetInt32(0);
                                Productos = reader.GetString(1);
                                Membresia = reader.GetString(2);
                                Encargado = reader.GetString(3);
                                Total = reader.GetDecimal(4);
                                Metodo = reader.GetString(5);
                            }
                            else
                            {
                                Response.Redirect("/Productos/Index");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                string connectionString = "Server=localhost\\SQLEXPRESS;Database=Panes;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "UPDATE Ventas SET productos=@Productos, membresia_cliente=@Membresia, nombre_encargado=@Encargado, total=@Total, metodo_pago=@Metodo WHERE Id_venta=@Id_venta";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Productos", Productos);
                        command.Parameters.AddWithValue("@Membresia", Membresia);
                        command.Parameters.AddWithValue("@Encargado", Encargado);
                        command.Parameters.AddWithValue("@Total", Total ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@Metodo", Metodo);
                        command.Parameters.AddWithValue("@Id_venta", Id_venta);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return RedirectToPage("/Productos/Index");
                        }
                        else
                        {
                            ErrorMessage = "No se pudo actualizar la venta. Por favor, inténtelo de nuevo.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }
    }
}
