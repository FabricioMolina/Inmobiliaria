using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MolinaInmobilaria.Models
{
	public class RepositorioInquilino
	{
        protected readonly string connectionString;
		public RepositorioInquilino()
		{
            connectionString="Server=localhost;User=root;Password=;Database=inmobiliariam;SslMode=none";
		}

		public int Alta(Inquilino p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inquilinos (Nombre, Apellido, Dni, Telefono, Email) " +
					$"VALUES (@nombre, @apellido, @dni, @telefono, @email);" +
					"SELECT LAST_INSERT_ID();";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", p.Nombre);
					command.Parameters.AddWithValue("@apellido", p.Apellido);
					command.Parameters.AddWithValue("@dni", p.Dni);
					command.Parameters.AddWithValue("@telefono", p.Telefono);
					command.Parameters.AddWithValue("@email", p.Email);
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
				string sql = $"DELETE FROM Inquilinos WHERE id = @id";
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
		public int Modificacion(Inquilino p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Inquilinos SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email " +
					$"WHERE id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", p.Nombre);
					command.Parameters.AddWithValue("@apellido", p.Apellido);
					command.Parameters.AddWithValue("@dni", p.Dni);
					command.Parameters.AddWithValue("@telefono", p.Telefono);
					command.Parameters.AddWithValue("@email", p.Email);
					command.Parameters.AddWithValue("@id", p.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inquilino> ObtenerTodos()
		{
			IList<Inquilino> res = new List<Inquilino>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, Nombre, Apellido, Dni, Telefono, Email FROM Inquilinos";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino p = new Inquilino
						{
							Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		public IList<Inquilino> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			IList<Inquilino> res = new List<Inquilino>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT id, Nombre, Apellido, Dni, Telefono, Email
					FROM Inquilinos
					LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
				";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino p = new Inquilino
						{
							Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		virtual public Inquilino ObtenerPorId(int id)
		{
			Inquilino p = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, Nombre, Apellido, Dni, Telefono, Email FROM Inquilinos" +
					$" WHERE id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int24).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Inquilino
						{
							Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
						};
					}
					connection.Close();
				}
			}
			return p;
        }

        public Inquilino ObtenerPorEmail(string email)
        {
            Inquilino p = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT id, Nombre, Apellido, Dni, Telefono, Email FROM Inquilinos" +
                    $" WHERE Email=@email";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }
		public Inquilino ObtenerPorTelefono(string telefono)
        {
            Inquilino p = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT id, Nombre, Apellido, Dni, Telefono, Email FROM Inquilinos" +
                    $" WHERE Telefono=@telefono";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@telefono", MySqlDbType.VarChar).Value = telefono;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }
		public Inquilino ObtenerPorDNI(string dni)
        {
            Inquilino p = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT id, Nombre, Apellido, Dni, Telefono, Email FROM Inquilinos" +
                    $" WHERE Dni=@dni";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@dni", MySqlDbType.VarChar).Value = dni;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        p = new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                        };
                    }
                    connection.Close();
                }
            }
            return p;
        }

        public IList<Inquilino> BuscarPorNombre(string nombre)
        {
            List<Inquilino> res = new List<Inquilino>();
            Inquilino p = null;
			nombre = "%" + nombre + "%";
			using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = $"SELECT id, nombre, apellido, dni, telefono, mail FROM inquilinos" +
                    $" WHERE nombre LIKE @nombre OR apellido LIKE @nombre";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@nombre", MySqlDbType.VarChar).Value = nombre;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        p = new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            Dni = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Email = reader.GetString(5),
                           
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

	}
}