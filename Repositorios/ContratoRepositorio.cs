using MySql.Data.MySqlClient;
using System.Data;
using MolinaInmobilaria.Models;

namespace MolinaInmobilaria.Repositorios
{

    public class ContratoRepositorio
    {
        protected readonly string connectionString;
        public ContratoRepositorio()
        {
            connectionString="Server=localhost;User=root;Password=;Database=inmobiliariam;SslMode=none";
        }
        public int Alta(Contrato c){
        int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				
				string sql = $"INSERT INTO Contratos (Id_Inquilino, Id_Inmueble, FechaInicio, Cantidad_Cuotas, FechaExpiracion) " +
					$"VALUES (@id_inquilino, @id_inmueble, @fechaInicio,@cantidad_cuotas, @fechaExpiracion);" +
					"SELECT LAST_INSERT_ID();";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id_inquilino", c.Id_Inquilino);
					command.Parameters.AddWithValue("@id_inmueble", c.Id_Inmueble);
					command.Parameters.AddWithValue("@fechaInicio",  c.FechaInicio);
					command.Parameters.AddWithValue("@cantidad_cuotas",  c.Cantidad_Cuotas);
					command.Parameters.AddWithValue("@fechaExpiracion", c.FechaExpiracion);
					
			        
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
                    c.Id = res;
                    connection.Close();
				}
			}
			return res;
        }
        public int Baja(int id)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Contratos WHERE id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
        public int Modificacion(Contrato c)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Contratos SET Id_Inquilino=@id_inquilino, Id_inmueble=@id_inmueble, FechaInicio=@fechaInicio,Cantidad_Cuotas=@cantidad_cuotas, FechaExpiracion=@fechaExpiracion" +
					$" WHERE id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", c.Id);
					command.Parameters.AddWithValue("@id_inquilino", c.Id_Inquilino);
					command.Parameters.AddWithValue("@id_inmueble", c.Id_Inmueble);

					command.Parameters.AddWithValue("@fechaInicio", c.FechaInicio);
					command.Parameters.AddWithValue("@cantidad_cuotas", c.Cantidad_Cuotas);
					command.Parameters.AddWithValue("@fechaExpiracion", c.FechaExpiracion);
					
			        
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
   
        public IList<Contrato> ObtenerTodos()
		{
			IList<Contrato> res = new List<Contrato>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{

				string sql = "SELECT c.id, Id_Inquilino, Id_Inmueble, FechaInicio, Cantidad_Cuotas, FechaExpiracion," +
					" i.Nombre, i.Apellido, m.Direccion, m.Precio" +
                    " FROM Contratos c INNER JOIN Inquilinos i ON c.Id_Inquilino = i.Id" +
					" INNER JOIN Inmuebles m ON c.Id_Inmueble = m.Id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato c = new Contrato
						{
							Id = reader.GetInt32(0),
                            Id_Inquilino = reader.GetInt32(1),
                            Id_Inmueble = reader.GetInt32(2),
                            FechaInicio = reader.GetDateTime(3),
							Cantidad_Cuotas = reader.GetInt32(4),
                            FechaExpiracion = reader.GetDateTime(5),
                            Inquilino = new Inquilino(){
								Id = reader.GetInt32(1),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),	
							},
							Inmueble = new Inmueble(){
								Id = reader.GetInt32(2),
								Direccion = reader.GetString(8),
                                Precio = reader.GetDouble(9),
							}
                            
						};
						res.Add(c);
					}
					connection.Close();
				}
			}
			return res;
		}
		
		public Contrato ObtenerPorId(int id){
			Contrato c = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{

				string sql = "SELECT c.id, Id_Inquilino, Id_Inmueble, FechaInicio, Cantidad_Cuotas, FechaExpiracion," +
					" i.Nombre, i.Apellido, m.Direccion, m.Precio" +
                    " FROM Contratos c INNER JOIN Inquilinos i ON c.Id_Inquilino = i.Id" +
					" INNER JOIN Inmuebles m ON c.Id_Inmueble = m.Id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						 c = new Contrato
						{
							Id = reader.GetInt32(0),
                            Id_Inquilino = reader.GetInt32(1),
                            Id_Inmueble = reader.GetInt32(2),
                            FechaInicio = reader.GetDateTime(3),
							Cantidad_Cuotas = reader.GetInt32(4),
                            FechaExpiracion = reader.GetDateTime(5),
                            Inquilino = new Inquilino(){
								Id = reader.GetInt32(1),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),	
							},
							Inmueble = new Inmueble(){
								Id = reader.GetInt32(2),
								Direccion = reader.GetString(8),
                                Precio = reader.GetDouble(9),
							}
                            
						};
						
					}
					connection.Close();
				}
			}
			return c;
		}
		public IList<Contrato> ObtenerInmueblesOcupados(string fechaInicio, string fechaExpiracion)
        {
			IList<Contrato> res = new List<Contrato>();           
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT c.id, Id_Inmueble " +
						"FROM Contratos c INNER JOIN Inmuebles i ON Id_Inmueble = i.Id " +
						"WHERE c.FechaInicio BETWEEN @fechaInicio AND @fechaExpiracion";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
					command.Parameters.AddWithValue("@fechaExpiracion", fechaExpiracion);
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato c = new Contrato
						{
							Id = reader.GetInt32(0),
                            Id_Inmueble = reader.GetInt32(1),                                                   
						};
						res.Add(c);
					}
					connection.Close();
				}
				
			}
			return res;
		}
		public IList<Contrato> ObtenerInmueblesNoOcupados(string fechaInicio, string fechaExpiracion)
        {
			IList<Contrato> res = new List<Contrato>();	
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT c.id, Id_Inmueble " +
						"FROM Contratos c INNER JOIN Inmuebles i ON Id_Inmueble = i.Id " +
						"WHERE c.FechaInicio NOT BETWEEN @fechaInicio AND @fechaExpiracion ";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					//command.Parameters.Add("@fechaInicio", MySqlDbType.Date).Value = fechaInicio;
					//command.Parameters.Add("@fechaExpiracion", MySqlDbType.Date).Value = fechaExpiracion;
					command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
					command.Parameters.AddWithValue("@fechaExpiracion", fechaExpiracion);
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato c = new Contrato
						{
							Id = reader.GetInt32(0),
                            Id_Inmueble = reader.GetInt32(1),
                            
                            
						};
						res.Add(c);
					}
					connection.Close();
				}
				
			}
			return res;
		}
	/*	 public IList<Contrato> ObtenerPorFechas(DateTime inicio, DateTime final)
        {
			IList<Contrato> res = new List<Contrato>();
            
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id_Inmueble" +
						"FROM Contratos c INNER JOIN Inmueble i ON Id_Inmueble = i.Id" +
						$" WHERE c.FechaInicio >=@inicio AND c.FechaExpiracion <= @final";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato c = new Contrato
						{
							Id = reader.GetInt32(0),
                            Id_Inquilino = reader.GetInt32(1),
                            Id_Inmueble = reader.GetInt32(2),
                            FechaInicio = reader.GetDateTime(3),
                            FechaExpiracion = reader.GetDateTime(4),
                            Inquilino = new Inquilino(){
								Id = reader.GetInt32(1),
                                Nombre = reader.GetString(5),
                                Apellido = reader.GetString(6),	
							},
							Inmueble = new Inmueble(){
								Id = reader.GetInt32(2),
								Direccion = reader.GetString(7),
                                
							}
                            
						};
						res.Add(c);
					}
					connection.Close();
				}
				
			}
			return res;
		}*/
		public IList<Contrato> ObtenerTodosVigentes()
		{
			IList<Contrato> res = new List<Contrato>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{

				string sql = "SELECT c.id, Id_Inquilino, Id_Inmueble, FechaInicio, Cantidad_Cuotas, FechaExpiracion," +
					" i.Nombre, i.Apellido, m.Direccion" +
                    " FROM Contratos c INNER JOIN Inquilinos i ON c.Id_Inquilino = i.Id" +
					" INNER JOIN Inmuebles m ON c.Id_Inmueble = m.Id" +
					" WHERE c.FechaExpiracion >= now()";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato c = new Contrato
						{
							Id = reader.GetInt32(0),
                            Id_Inquilino = reader.GetInt32(1),
                            Id_Inmueble = reader.GetInt32(2),
                            FechaInicio = reader.GetDateTime(3),
							Cantidad_Cuotas = reader.GetInt32(4),
                            FechaExpiracion = reader.GetDateTime(5),
                            Inquilino = new Inquilino(){
								Id = reader.GetInt32(1),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),	
							},
							Inmueble = new Inmueble(){
								Id = reader.GetInt32(2),
								Direccion = reader.GetString(8),
                                
							}
                            
						};
						res.Add(c);
					}
					connection.Close();
				}
			}
			return res;
		}
		public IList<Contrato> ObtenerTodosNoVigentes()
		{
			IList<Contrato> res = new List<Contrato>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{

				string sql = "SELECT c.id, Id_Inquilino, Id_Inmueble, FechaInicio, Cantidad_Cuotas,FechaExpiracion," +
					" i.Nombre, i.Apellido, m.Direccion" +
                    " FROM Contratos c INNER JOIN Inquilinos i ON c.Id_Inquilino = i.Id" +
					" INNER JOIN Inmuebles m ON c.Id_Inmueble = m.Id" +
					" WHERE c.FechaExpiracion <= now()";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato c = new Contrato
						{
							Id = reader.GetInt32(0),
                            Id_Inquilino = reader.GetInt32(1),
                            Id_Inmueble = reader.GetInt32(2),
                            FechaInicio = reader.GetDateTime(3),
							Cantidad_Cuotas = reader.GetInt32(4),
                            FechaExpiracion = reader.GetDateTime(5),
                            Inquilino = new Inquilino(){
								Id = reader.GetInt32(1),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),	
							},
							Inmueble = new Inmueble(){
								Id = reader.GetInt32(2),
								Direccion = reader.GetString(8),
                                Precio = reader.GetFloat(9),
							}
                            
						};
						res.Add(c);
					}
					connection.Close();
				}
			}
			return res;
		}
		
		public IList<Contrato> ObtenerPorInmueble(int id)
        {
            List<Contrato> res = new List<Contrato>();
            
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = "SELECT c.id, Id_Inquilino, Id_Inmueble, FechaInicio, Cantidad_Cuotas,FechaExpiracion," +
					" i.Nombre, i.Apellido, m.Direccion" +
                    " FROM Contratos c INNER JOIN Inquilinos i ON c.Id_Inquilino = i.Id" +
					" INNER JOIN Inmuebles m ON c.Id_Inmueble = m.Id" +
					" WHERE c.Id_Inmueble = @id  ORDER BY FechaInicio DESC";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato c = new Contrato
						{
							Id = reader.GetInt32(0),
                            Id_Inquilino = reader.GetInt32(1),
                            Id_Inmueble = reader.GetInt32(2),
                            FechaInicio = reader.GetDateTime(3),
							Cantidad_Cuotas = reader.GetInt32(4),
                            FechaExpiracion = reader.GetDateTime(5),
                            Inquilino = new Inquilino(){
								Id = reader.GetInt32(1),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),	
							},
							Inmueble = new Inmueble(){
								Id = reader.GetInt32(2),
								Direccion = reader.GetString(8),
                                
							}
                            
						};
						res.Add(c);
					}
					connection.Close();
				}
			}
			return res;
		}
		
    }
        

    
}