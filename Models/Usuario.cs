using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MolinaInmobilaria.Models{
    public enum enRoles{
        Admin = 1,
        Empleado = 2,
    }

    public class Usuario{
        
        public int Id {get;set;}
        [Required]
		public string Nombre { get; set; }
		[Required]
		public string Apellido { get; set; }
		[Required, EmailAddress]
		public string Email { get; set; }
		[Required, DataType(DataType.Password)]
		public string Clave { get; set; }
		public string? Avatar { get; set; }
		[NotMapped]
		public IFormFile AvatarFile { get; set; }
		public int Rol { get; set; }
		[NotMapped]
		public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "";

		public static IDictionary<int, string> ObtenerRoles()
		{
			SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
			Type tipoEnumRol = typeof(enRoles);
			foreach (var valor in Enum.GetValues(tipoEnumRol))
			{
				roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
			}
			return roles;
		}
    }
}