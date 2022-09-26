namespace MolinaInmobilaria.Models;

public class Inmueble
{
    public int Id { get; set; }
	public int Id_Propietario { get; set; }
	public string Direccion { get; set; }
	public string Ambientes { get; set; }
	public string Latitud { get; set; }
	public string Longitud { get; set; }
	public string Tipo { get; set; }
	public string Uso { get; set; }
	public double Precio { get; set; }
    public int Estado { get; set; }
	public Propietario? Propietario {get;set;}
}
