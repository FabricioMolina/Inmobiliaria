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
	public class PagoRepositorio
	{
        protected readonly string connectionString;
		public PagoRepositorio()
		{
            connectionString="Server=localhost;User=root;Password=;Database=inmobiliariam;SslMode=none";
		}

		public int Alta(Pago p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Pagos (Id_Contrato, Fecha, Monto, Cantidad_Cuotas, Numero_Cuota, Tipo_Pago) " +
					$"VALUES (@id_contrato, @fecha, @monto, @cantidad_cuotas, @numero_cuota, @tipo_pago);" +
					"SELECT LAST_INSERT_ID();";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id_contrato", p.Id_Contrato);
					command.Parameters.AddWithValue("@fecha", p.Fecha);
					command.Parameters.AddWithValue("@monto", p.Monto);
					command.Parameters.AddWithValue("@cantidad_cuotas", p.Cantidad_Cuotas);
					command.Parameters.AddWithValue("@numero_cuota", p.Numero_Cuota);
					command.Parameters.AddWithValue("@tipo_pago", p.Tipo_Pago);
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
				string sql = $"DELETE FROM Pagos WHERE id = @id";
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
		public int Modificacion(Pago p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"UPDATE Pagos SET Id_Contrato=@id_contrato, Fecha=@fecha, Monto=@monto, Tipo_Pago=@tipo_pago " +
				"Cantidad_Cuotas=@cantidad_cuotas, Numero_Cuota=@numero_cuota" +
					$"WHERE id = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id_contrato", p.Id_Contrato);
					command.Parameters.AddWithValue("@fecha", p.Fecha);
					command.Parameters.AddWithValue("@monto", p.Monto);
					command.Parameters.AddWithValue("@cantidad_cuotas", p.Cantidad_Cuotas);
					command.Parameters.AddWithValue("@numero_cuota", p.Numero_Cuota);
					command.Parameters.AddWithValue("@tipo_pago", p.Tipo_Pago);
					command.Parameters.AddWithValue("@id", p.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public IList<Pago> ObtenerVencidos(int id){
			IList<Pago> res = new List<Pago>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, Id_Contrato, Fecha, Monto,Cantidad_Cuotas, Numero_Cuota, Tipo_Pago FROM Pagos WHERE Id_Contrato=@id AND tipo_pago = 'Vencida'";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int24).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago p = new Pago
						{
							Id = reader.GetInt32(0),
                            Id_Contrato = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Monto = reader.GetDouble(3),
							Cantidad_Cuotas = reader.GetInt32(4),
							Numero_Cuota = reader.GetInt32(5),
							Tipo_Pago = reader.GetString(6),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		public IList<Pago> ObtenerRepetidos(int id)
		{
			IList<Pago> res = new List<Pago>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, Id_Contrato, Fecha, Monto,Cantidad_Cuotas, Numero_Cuota, Tipo_Pago FROM Pagos WHERE Id_Contrato=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int24).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago p = new Pago
						{
							Id = reader.GetInt32(0),
                            Id_Contrato = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Monto = reader.GetDouble(3),
							Cantidad_Cuotas = reader.GetInt32(4),
							Numero_Cuota = reader.GetInt32(5),
							Tipo_Pago = reader.GetString(6),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}
		public IList<Pago> ObtenerTodos()
		{
			IList<Pago> res = new List<Pago>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, Id_Contrato, Fecha, Monto,Cantidad_Cuotas,Numero_Cuota, Tipo_Pago FROM Pagos";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago p = new Pago
						{
							Id = reader.GetInt32(0),
                            Id_Contrato = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Monto = reader.GetDouble(3),
							Cantidad_Cuotas = reader.GetInt32(4),
							Numero_Cuota = reader.GetInt32(5),
							Tipo_Pago= reader.GetString(6),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}
		virtual public Pago ObtenerPorId(int id)
		{
			Pago p = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, Id_Contrato, Fecha, Monto,Cantidad_Cuotas,Numero_Cuota, Tipo_Pago FROM Pagos" +
					$" WHERE id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int24).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Pago
						{
							Id = reader.GetInt32(0),
                            Id_Contrato = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Monto = reader.GetDouble(3),
							Cantidad_Cuotas = reader.GetInt32(4),
							Numero_Cuota = reader.GetInt32(5),
							Tipo_Pago= reader.GetString(6),
						};
					}
					connection.Close();
				}
			}
			return p;
        }
		virtual public Pago ObtenerFinalizado(string tipo_pago)
		{
			Pago p = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, Id_Contrato, Fecha, Monto,Cantidad_Cuotas,Numero_Cuota, Tipo_Pago FROM Pagos" +
					$" WHERE tipo_pago=@tipo_pago";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@tipo_pago", MySqlDbType.String).Value = tipo_pago;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Pago
						{
							Id = reader.GetInt32(0),
                            Id_Contrato = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Monto = reader.GetDouble(3),
							Cantidad_Cuotas = reader.GetInt32(4),
							Numero_Cuota = reader.GetInt32(5),
							Tipo_Pago= reader.GetString(6),
						};
					}
					connection.Close();
				}
			}
			return p;
        }
		/*virtual public Pago ObtenerPorContrato(int id)
		{
			Pago p = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT id, Id_Contrato, Fecha, Monto FROM Pagos" +
					$" WHERE Id_Contrato=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int24).Value = id;
                    command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Pago
						{
							Id = reader.GetInt32(0),
                            Id_Contrato = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Monto = reader.GetFloat(3),
						};
					}
					connection.Close();
				}
			}
			return p;
        }
		
		*/
	}
}