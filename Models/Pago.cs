namespace MolinaInmobilaria.Models;

public class Pago
{
    public int Id { get; set; }
    public int Id_Contrato {get;set;}
	public DateTime Fecha {get;set;}
    public Double Monto {get;set;}
    public int Cantidad_Cuotas { get; set; }
    public int Numero_Cuota { get; set; }
    public string Tipo_Pago { get; set; }
}
