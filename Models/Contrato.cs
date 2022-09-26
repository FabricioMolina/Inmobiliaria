namespace MolinaInmobilaria.Models;

public class Contrato
{
    public int Id { get; set; }
	public int Id_Inquilino {get;set;}
    public int Id_Inmueble {get;set;}
    public DateTime FechaInicio {get;set;}
    public int Cantidad_Cuotas {get;set;}
    public DateTime FechaExpiracion {get;set;}
    public Inquilino? Inquilino {get;set;}
    public Inmueble? Inmueble {get;set;}
    
    
}
