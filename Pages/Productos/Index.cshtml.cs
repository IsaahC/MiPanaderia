using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Panaderia.Pages.Productos
{
    public class Index : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Index> _logger;

        public Index(IConfiguration configuration, ILogger<Index> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public List<VentaInfo> VentaList { get; set; } = new List<VentaInfo>();

        public void OnGet()
        {
            try
            {
                string connectionString = "Server=localhost\\SQLEXPRESS;Database=Panes;Trusted_Connection=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Ventas";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VentaInfo ventaInfo = new VentaInfo();

                                ventaInfo.Id_venta = reader.GetInt32(0);
                                ventaInfo.Productos = reader.GetString(1);
                                ventaInfo.Membresia = reader.GetString(2);
                                ventaInfo.Encargado = reader.GetString(3);
                                ventaInfo.Total = reader.GetDecimal(4);
                                ventaInfo.Metodo = reader.GetString(5);
                                ventaInfo.FechaH = reader.GetDateTime(6).ToString("MM/dd/yyyy");
                                VentaList.Add(ventaInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hay un error" + ex.Message);
            }
        }
    }

    public class VentaInfo
    {
        public int Id_venta { get; set; }
        public string Productos { get; set; } = "";
        public string Membresia { get; set; } = "";
        public string Encargado { get; set; } = "";
        public decimal Total { get; set; }
        public string FechaH { get; set; } = "";
        public string Metodo { get; set; } = "";
    }
}
