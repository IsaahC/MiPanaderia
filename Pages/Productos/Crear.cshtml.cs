using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Panaderia.Pages.Productos
{
  
    public class Crear : PageModel
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

        

        public void OnGet()
        {
        }

        public string ErrorMessage { get; set; } = "";
         public void OnPost()
        {
            if(!ModelState.IsValid){
                return;
            }

            if(Productos == null) Productos = "";
            if(Membresia == null) Membresia = "";
            if(Encargado == null) Encargado = "";
            if(Metodo == null) Metodo = "";
            

            //crear nuevo cliente
            try
            {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=Panes;Trusted_Connection=True;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(connectionString)){
                connection.Open();

                String sql = "INSERT INTO Ventas " + 
                "(productos, membresia_cliente, nombre_encargado, total, metodo_pago) VALUES" +
                "(@productos, @membresia_cliente, @nombre_encargado, @total, @metodo_pago)";
            using(SqlCommand command = new SqlCommand(sql, connection)){
                command.Parameters.AddWithValue("@productos", Productos);
                command.Parameters.AddWithValue("@membresia_cliente", Membresia);
                command.Parameters.AddWithValue("@nombre_encargado", Encargado);
                command.Parameters.AddWithValue("@total", Total);
                command.Parameters.AddWithValue("@metodo_pago", Metodo);
                
                command.ExecuteNonQuery();
            }
            } 
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Productos/Index");
        }
    }
}