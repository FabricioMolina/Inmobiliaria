using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MolinaInmobilaria.Models;


namespace MolinaInmobilaria.Repositorios{

	public interface IRepositorioUsuario : IRepositorio<Usuario>
	{
		Usuario ObtenerPorEmail(string email);
    }
}