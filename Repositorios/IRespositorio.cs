using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolinaInmobilaria.Models;
namespace MolinaInmobilaria.Repositorios;

	public interface IRepositorio<T>
	{
		int Alta(T p);
		int Baja(int id);
		int Modificacion(T p);

		IList<T> ObtenerTodos();
		T ObtenerPorId(int id);
	}