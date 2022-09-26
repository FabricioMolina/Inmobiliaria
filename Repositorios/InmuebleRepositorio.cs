using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MolinaInmobilaria.Models;

namespace MolinaInmobilaria.Repositorios
{
	public class InmuebleRepositorio
	{
        protected readonly string connectionString;
		public InmuebleRepositorio()
		{
            connectionString="Server=localhost;User=root;Password=;Database=inmobiliariam;SslMode=none";
		}

		public int Alta(Inmueble p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inmuebles (Id_Propietario, Direccion, Ambientes, Latitud, Longitud , Tipo, Uso, Precio, Estado) " +
					$"VALUES (@id_propietario, @direccion, @ambientes,  @latitud, @longitud , @tipo, @uso, @precio, @estado);" +
					"SELECT LAST_INSERT_ID();";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id_propietario", p.Id_Propietario);
					command.Parameters.AddWithValue("@direccion", p.Direccion);
					command.Parameters.AddWithValue("@ambientes", p.Ambientes);
					command.Parameters.AddWithValue("@latitud", p.Latitud);
					command.Parameters.AddWithValue("@longitud", p.Longitud);
					command.Parameters.AddWithValue("@tipo", p.Tipo);
					command.Parameters.AddWithValue("@uso", p.Uso);
					command.Parameters.AddWithValue("@precio", p.Precio);
					command.Parameters.AddWithValue("@estado", p.Estado);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
                    p.Id = res;
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
				string sql = $"DELETE FROM Inmuebles WHERE id = @id";
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
		public int Modificacion(Inmueble p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Inmuebles SET Id_Propietario=@id_propietario, Direccion=@direccion, Ambientes=@ambientes, Latitud=@latitud, Longitud=@longitud,Tipo=@tipo,Uso=@uso, Precio=@precio, Estado=@estado " +
					$"WHERE id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id_propietario", p.Id_Propietario);
					command.Parameters.AddWithValue("@direccion", p.Direccion);
					command.Parameters.AddWithValue("@ambientes", p.Ambientes);
					command.Parameters.AddWithValue("@latitud", p.Latitud);
					command.Parameters.AddWithValue("@Longitud", p.Longitud);				
					command.Parameters.AddWithValue("@tipo", p.Tipo);
					command.Parameters.AddWithValue("@uso", p.Uso);
					command.Parameters.AddWithValue("@precio", p.Precio);
					command.Parameters.AddWithValue("@estado", p.Estado);
					command.Parameters.AddWithValue("@id", p.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inmueble> ObtenerTodos()
		{
			IList<Inmueble> res = new List<Inmueble>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				//string sql = $"SELECT i.id, Id_Propietario, Direccion, Ambientes, Latitud, Longitud , Tipo, Uso, Precio, Estado,"+
				//"p.Nombre, p.Apellido" +  "FROM Inmuebles i INNER JOIN Propietarios p ON i.Id_Propietario = p.Id";
				string sql = "SELECT i.id, Id_Propietario, Direccion, Ambientes, Latitud, Longitud , Tipo, Uso, Precio, Estado," +
					" p.Nombre, p.Apellido" +
                    " FROM Inmuebles i INNER JOIN Propietarios p ON i.Id_Propietario = p.Id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble inmueble = new Inmueble
						{
							Id = reader.GetInt32(0),
                            Id_Propietario = reader.GetInt32(1),
                            Direccion = reader.GetString(2),
                            Ambientes = reader.GetString(3),
							Latitud = reader.GetString(4),
							Longitud = reader.GetString(5),
							Tipo = reader.GetString(6),
							Uso = reader.GetString(7),
                            Precio = reader.GetFloat(8),
                            Estado = reader.GetInt32(9),
							Propietario = new Propietario(){
								Id = reader.GetInt32(1),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),	
							}
						};
						res.Add(inmueble);
						};
						
					}
					connection.Close();
				}
			
			return res;
		}
		virtual public Inmueble ObtenerPorId(int id)
		{
			Inmueble p = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = "SELECT i.id, Id_Propietario, Direccion, Ambientes, Latitud, Longitud , Tipo, Uso, Precio, Estado," +
					" p.Nombre, p.Apellido" +
                    " FROM Inmuebles i INNER JOIN Propietarios p ON i.Id_Propietario = p.Id" +
					$" WHERE i.id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int24).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Inmueble
						{
							Id = reader.GetInt32(0),
                            Id_Propietario = reader.GetInt32(1),
                            Direccion = reader.GetString(2),
                            Ambientes = reader.GetString(3),
							Latitud = reader.GetString(4),
							Longitud = reader.GetString(5),
							Tipo = reader.GetString(6),
							Uso = reader.GetString(7),
                            Precio = reader.GetFloat(8),
                            Estado = reader.GetInt32(9),
							Propietario = new Propietario(){
								Id = reader.GetInt32(1),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),	
							}
						};
					}
					connection.Close();
				}
			}
			return p;
        }

        public Inmueble ObtenerPorEmail(string email)
        {
            Inmueble p = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, Id_Propietario, Direccion, Ambientes, Latitud, Longitud , Tipo, Uso, Precio, Estado FROM Inmuebles" +
					$" WHERE email=@email";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Inmueble
						{
							Id = reader.GetInt32(0),
                            Id_Propietario = reader.GetInt32(1),
                            Direccion = reader.GetString(2),
                            Ambientes = reader.GetString(3),
							Latitud = reader.GetString(4),
							Longitud = reader.GetString(5),
							Tipo = reader.GetString(6),
							Uso = reader.GetString(7),
                            Precio = reader.GetFloat(8),
                            Estado = reader.GetInt32(9),
						};
					}
					connection.Close();
				}
			}
			return p;
        }
		public IList<Inmueble> ObtenerDisponibles()
        {
            IList<Inmueble> res = new List<Inmueble>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = "SELECT i.id, Id_Propietario, Direccion, Ambientes, Latitud, Longitud , Tipo, Uso, Precio, Estado," +
					" p.Nombre, p.Apellido" +
                    " FROM Inmuebles i INNER JOIN Propietarios p ON i.Id_Propietario = p.Id"+
					$" WHERE estado=1";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble inmueble = new Inmueble
						{
							Id = reader.GetInt32(0),
                            Id_Propietario = reader.GetInt32(1),
                            Direccion = reader.GetString(2),
                            Ambientes = reader.GetString(3),
							Latitud = reader.GetString(4),
							Longitud = reader.GetString(5),
							Tipo = reader.GetString(6),
							Uso = reader.GetString(7),
                            Precio = reader.GetFloat(8),
                            Estado = reader.GetInt32(9),
							Propietario = new Propietario(){
								Id = reader.GetInt32(1),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),	
							}
						};
						res.Add(inmueble);
						};
						
					}
					connection.Close();
				}
			
			return res;
		}
		public IList<Inmueble> ObtenerVencidos()
        {
            IList<Inmueble> res = new List<Inmueble>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = "SELECT i.id, Id_Propietario, Direccion, Ambientes, Latitud, Longitud , Tipo, Uso, Precio, Estado," +
					" p.Nombre, p.Apellido" +
                    " FROM Inmuebles i INNER JOIN Propietarios p ON i.Id_Propietario = p.Id"+
					$" WHERE estado=1";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble inmueble = new Inmueble
						{
							Id = reader.GetInt32(0),
                            Id_Propietario = reader.GetInt32(1),
                            Direccion = reader.GetString(2),
                            Ambientes = reader.GetString(3),
							Latitud = reader.GetString(4),
							Longitud = reader.GetString(5),
							Tipo = reader.GetString(6),
							Uso = reader.GetString(7),
                            Precio = reader.GetFloat(8),
                            Estado = reader.GetInt32(9),
							Propietario = new Propietario(){
								Id = reader.GetInt32(1),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),	
							}
						};
						res.Add(inmueble);
						};
						
					}
					connection.Close();
				}
			
			return res;
		}
		
		public IList<Inmueble> ObtenerPorPropietario(int id)
        {
            List<Inmueble> res = new List<Inmueble>();
            
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = "SELECT i.id, Id_Propietario, Direccion, Ambientes, Latitud, Longitud , Tipo, Uso, Precio, Estado," +
					" p.Nombre, p.Apellido" +
                    " FROM Inmuebles i INNER JOIN Propietarios p ON i.Id_Propietario = p.Id"+
					$" WHERE id_propietario = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble inmueble = new Inmueble
						{
							Id = reader.GetInt32(0),
                            Id_Propietario = reader.GetInt32(1),
                            Direccion = reader.GetString(2),
                            Ambientes = reader.GetString(3),
							Latitud = reader.GetString(4),
							Longitud = reader.GetString(5),
							Tipo = reader.GetString(6),
							Uso = reader.GetString(7),
                            Precio = reader.GetFloat(8),
                            Estado = reader.GetInt32(9),
							Propietario = new Propietario(){
								Id = reader.GetInt32(1),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),	
							}
						};
						res.Add(inmueble);
						};
						
					}
					connection.Close();
				}
			
			return res;
		}
		public IList<Inmueble> ObtenerPorTipo(string tipo)
        {
            List<Inmueble> res = new List<Inmueble>();
            
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = "SELECT i.id, Id_Propietario, Direccion, Ambientes, Latitud, Longitud , Tipo, Uso, Precio, Estado," +
					" p.Nombre, p.Apellido" +
                    " FROM Inmuebles i INNER JOIN Propietarios p ON i.Id_Propietario = p.Id"+
					$" WHERE tipo LIKE @tipo";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@tipo", MySqlDbType.String).Value = tipo;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble inmueble = new Inmueble
						{
							Id = reader.GetInt32(0),
                            Id_Propietario = reader.GetInt32(1),
                            Direccion = reader.GetString(2),
                            Ambientes = reader.GetString(3),
							Latitud = reader.GetString(4),
							Longitud = reader.GetString(5),
							Tipo = reader.GetString(6),
							Uso = reader.GetString(7),
                            Precio = reader.GetFloat(8),
                            Estado = reader.GetInt32(9),
							Propietario = new Propietario(){
								Id = reader.GetInt32(1),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),	
							}
						};
						res.Add(inmueble);
						};
						
					}
					connection.Close();
				}
			
			return res;
		}
		public IList<Inmueble> ObtenerPorUso(string uso)
        {
            List<Inmueble> res = new List<Inmueble>();
            
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = "SELECT i.id, Id_Propietario, Direccion, Ambientes, Latitud, Longitud , Tipo, Uso, Precio, Estado," +
					" p.Nombre, p.Apellido" +
                    " FROM Inmuebles i INNER JOIN Propietarios p ON i.Id_Propietario = p.Id"+
					$" WHERE uso LIKE @uso";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@uso", MySqlDbType.String).Value = uso;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble inmueble = new Inmueble
						{
							Id = reader.GetInt32(0),
                            Id_Propietario = reader.GetInt32(1),
                            Direccion = reader.GetString(2),
                            Ambientes = reader.GetString(3),
							Latitud = reader.GetString(4),
							Longitud = reader.GetString(5),
							Tipo = reader.GetString(6),
							Uso = reader.GetString(7),
                            Precio = reader.GetFloat(8),
                            Estado = reader.GetInt32(9),
							Propietario = new Propietario(){
								Id = reader.GetInt32(1),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),	
							}
						};
						res.Add(inmueble);
						};
						
					}
					connection.Close();
				}
			
			return res;
		}
		public IList<Inmueble> ObtenerPorTipoYUso(string tipo, string uso)
        {
            List<Inmueble> res = new List<Inmueble>();
            
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = "SELECT i.id, Id_Propietario, Direccion, Ambientes, Latitud, Longitud , Tipo, Uso, Precio, Estado," +
					" p.Nombre, p.Apellido" +
                    " FROM Inmuebles i INNER JOIN Propietarios p ON i.Id_Propietario = p.Id"+
					$" WHERE tipo LIKE @tipo AND uso LIKE @uso";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@uso", MySqlDbType.String).Value = uso;
					command.Parameters.Add("@tipo", MySqlDbType.String).Value = tipo;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble inmueble = new Inmueble
						{
							Id = reader.GetInt32(0),
                            Id_Propietario = reader.GetInt32(1),
                            Direccion = reader.GetString(2),
                            Ambientes = reader.GetString(3),
							Latitud = reader.GetString(4),
							Longitud = reader.GetString(5),
							Tipo = reader.GetString(6),
							Uso = reader.GetString(7),
                            Precio = reader.GetFloat(8),
                            Estado = reader.GetInt32(9),
							Propietario = new Propietario(){
								Id = reader.GetInt32(1),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),	
							}
						};
						res.Add(inmueble);
						};
						
					}
					connection.Close();
				}
			
			return res;
		}
    }
}