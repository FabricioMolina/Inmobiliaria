using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MolinaInmobilaria.Models;
using MolinaInmobilaria.Repositorios;

namespace MolinaInmobilaria.Controllers
{
    public class ContratoController : Controller
    
    {
          ContratoRepositorio repo = new ContratoRepositorio();
          RepositorioInquilino repoInquilino = new RepositorioInquilino();
          InmuebleRepositorio repoInmueble = new InmuebleRepositorio();
          PagoRepositorio repoPago = new PagoRepositorio();
        
        // GET: Contrato
        [Authorize]
        public ActionResult Index()
        {
            
            var aux = repo.ObtenerTodos();
            return View(aux);
        }
        [Authorize]
        public ActionResult ObtenerTodosVigentes()
        {
            ViewBag.Msj = "Todos nuestro Contratos Vigentes.";
            var aux = repo.ObtenerTodosVigentes();
            return View("Index", aux);
        }
        [Authorize]
        public ActionResult ObtenerTodosNoVigentes()
        {
            ViewBag.Msj = "Todos nuestro Contratos NO Vigentes.";
            var aux = repo.ObtenerTodosNoVigentes();
            return View("Index", aux);
        }
        [Authorize]
        public ActionResult ObtenerPorInmueble(int id)
        {
            var aux = repo.ObtenerPorInmueble(id);
            var inmueble = repoInmueble.ObtenerPorId(id);
            
            return View("Index", aux);
        }
        [Authorize]
        public ActionResult Create(int id)
        {
            
            var listInquilino = repoInquilino.ObtenerTodos();
            var inmueble = repoInmueble.ObtenerPorId(id);

            ViewBag.inmueblesOcupados = repo.ObtenerPorInmueble(id);

            ViewBag.inmuebles = inmueble;
            ViewBag.inquilino = listInquilino;
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato c)
        {
            
            if(!ModelState.IsValid){
                return View();
            }else{
               // c.FechaExpiracion = c.FechaInicio.AddMonths()
                List<Contrato> contratos = new List<Contrato>();
                DateTime final = c.FechaInicio.AddMonths(c.Cantidad_Cuotas);
                var ocupados = repo.ObtenerInmueblesOcupados(c.FechaInicio.ToString("yyyy-MM-dd"), final.ToString("yyyy-MM-dd"));      
                    try
                    {
                        foreach (var item in ocupados)
                        {
                            if(c.Id_Inmueble == item.Id_Inmueble){
                                contratos.Add(item);
                            }
                        }
                        if(contratos.Count() == 0){
                            DateTime expirar = c.FechaInicio.AddMonths(c.Cantidad_Cuotas);
                            c.FechaExpiracion = expirar;    
                            var alta = repo.Alta(c);
                            ViewBag.Mensaje = "Contrato creado perfectamente";
                            return RedirectToAction(nameof(Index));
                        }else{
                            ViewBag.Error = "El inmueble esta ocupado en ese margen de fechas.";
                            return View("Paso1");
                        }
                        
                
                        
                    }
                catch(Exception ex)
                    {
                        throw;
                    }
                
            
            }                       
        }

        [Authorize]
        
        public ActionResult Renovar(int id){
            
            var contrato = repo.ObtenerPorId(id);
            ViewBag.inmueblesOcupados = repo.ObtenerPorInmueble(contrato.Id_Inmueble);
            var inquilino = repoInquilino.ObtenerPorId(contrato.Id_Inquilino);
            var inmueble = repoInmueble.ObtenerPorId(contrato.Id_Inmueble);

            ViewBag.Inquilino = inquilino;
            ViewBag.Inmueble = inmueble;
            return View(contrato);
        }
        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public ActionResult Renovar(int id, Contrato c){          
            var contrato = repo.ObtenerPorId(id);
            var allContratos = repo.ObtenerTodos();
            List<Contrato> contratos = new List<Contrato>();
            DateTime final = c.FechaInicio.AddMonths(c.Cantidad_Cuotas);
            var ocupados = repo.ObtenerInmueblesOcupados(c.FechaInicio.ToString("yyyy-MM-dd"), final.ToString("yyyy-MM-dd"));

            foreach (var item in ocupados)
                {
                    if(c.Id_Inmueble == item.Id_Inmueble){
                        contratos.Add(item);
                    }
                }
             if(contratos.Count() == 0){
                DateTime expirar = c.FechaInicio.AddMonths(c.Cantidad_Cuotas);
                c.FechaExpiracion = expirar;    
                var alta = repo.Alta(c);
                ViewBag.Mensaje = "Contrato Creado perfectamente";
                return RedirectToAction(nameof(Index));
             }else{
                ViewBag.Error = "El inmueble esta ocupado en ese margen de fechas.";
                return View("Index", allContratos);
            }
            
            
        }
        [Authorize]
        public ActionResult Paso1()
        {         
            return View();
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Paso1(Inmueble i)
        {

        var tipo = i.Tipo;
        var uso = i.Uso;
        var inmueble = repoInmueble.ObtenerPorTipoYUso(tipo, uso);
        if(inmueble.Count == 0){
            ViewBag.Data = "No contamos con un Inmueble con esas especificaciones, estos son todos los que tenemos disponibles:";
            var aux = repoInmueble.ObtenerTodos();           
            return View("Paso2", aux);
        }else{
            ViewBag.Data = "Nuestros inmuebles en base a su eleccion";
            return View("Paso2", inmueble);
        }
        
        }
        [Authorize]
        public ActionResult Paso2()
        {
            var aux = repoInmueble.ObtenerTodos();  
            return View(aux);
        }
        [Authorize]
        public ActionResult Details(int id)
        {
            var contrato = repo.ObtenerPorId(id);
            return View(contrato);
        }
        [Authorize]
        public ActionResult Cancelar(int id)
        {
            var contrato = repo.ObtenerPorId(id);
            return View(contrato);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancelar(int id, Contrato c)
        {
            try
            {
                Pago pago = new Pago();
                Contrato contrato = repo.ObtenerPorId(id);
                
                IList<Pago> repetidos  = repoPago.ObtenerRepetidos(id);
                IList<Pago> vencidas = repoPago.ObtenerVencidos(id);
                var cantidad =  repetidos.Count();
                var cuotasVencidas = vencidas.Count();
                
            
                if(cantidad == 0){
                    pago.Numero_Cuota = 1;   
                    cantidad = 1;
                    ViewBag.Cantidad = cantidad;   
                }else{
                    pago.Numero_Cuota = cantidad +1;
                    cantidad = cantidad +1;
                    ViewBag.Cantidad = cantidad;
                }
                if(contrato.Cantidad_Cuotas/2 > cantidad){
                    
                    if(cuotasVencidas == 0){
                        pago.Monto = contrato.Inmueble.Precio*2;
                    }else{
                        var multa = contrato.Inmueble.Precio*0.25;
                        pago.Monto = (contrato.Inmueble.Precio*2) + (contrato.Inmueble.Precio*cuotasVencidas + multa*cuotasVencidas); 
                    }
                    
                }else{
                    if(cuotasVencidas == 0){
                        pago.Monto = contrato.Inmueble.Precio;
                    }else{
                        var multa = contrato.Inmueble.Precio*0.25;
                        double total = (contrato.Inmueble.Precio) + (contrato.Inmueble.Precio*cuotasVencidas + multa*cuotasVencidas); 
                        pago.Monto = total;
                    }
                }

                
                pago.Id_Contrato = id;
                pago.Fecha = DateTime.Now;
                pago.Cantidad_Cuotas = contrato.Cantidad_Cuotas;
                pago.Tipo_Pago = "Contrato Cancelado.";



                repoPago.Alta(pago);

                repo.Baja(id);
                ViewBag.Mensaje = "El Contrato fue cancelado y fue dado de baja, se creo un registro el pago.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw;
            }
        }
        [Authorize]
        public ActionResult Edit(int id)
        {   
            IList<Pago> pago = repoPago.ObtenerRepetidos(id);
            var cantidad =  pago.Count();
            int auxiliar = 0;
            foreach (var item in pago)
            {
                if(item.Tipo_Pago == "Contrato Finalizado"){
                    auxiliar = 1;
                }
            }
            if(auxiliar == 1){
                ViewBag.Finalizado = true;
            }else{
                ViewBag.Finalizado = false;
            }
            if(cantidad == 0){
                ViewBag.Cantidad = 0;
            }else{
                ViewBag.Error = "No se puede editar un contrato en el que ya pagaste una cuota.";
            }
            var inquilino = repoInquilino.ObtenerTodos();
            var inmueble = repoInmueble.ObtenerTodos();
            var contrato = repo.ObtenerPorId(id);
            ViewBag.inquilino = inquilino;
            ViewBag.inmueble = inmueble;

            return View(contrato);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato c)
        {
            Contrato contrato = null;
            try
            {
                contrato.Cantidad_Cuotas = c.Cantidad_Cuotas;
                contrato.FechaInicio = c.FechaInicio;
                contrato.Id = c.Id;
                contrato.Id_Inmueble = c.Id_Inmueble;
                contrato.Id_Inquilino = c.Id_Inquilino;
                contrato.FechaExpiracion = c.FechaInicio.AddMonths(c.Cantidad_Cuotas);
                repo.Modificacion(contrato);
                ViewBag.Mensaje = "Contrato modificado";                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw;
            }
        }

        [Authorize(Policy="Admin")]
        public ActionResult Delete(int id)
        {
            var c = repo.ObtenerPorId(id);
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy="Admin")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repo.Baja(id);
                ViewBag.Mensaje = "El contrato con Codigo " + id + " fue eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw;
            }
        }
      /*  public ActionResult Pagar(int id)
        {
            var user = repo.ObtenerPorId(id);
            return View("~/Views/Pago/Create.cshtml");
        }
        public ActionResult PagarContrato(int id)
        {
            var user = repo.ObtenerPorId(id);
            var c = repo.ObtenerTodos();
            if(user.FechaExpiracion < DateTime.Now){
                return View("Index", c);
            }else{
                Pago pago = new Pago();
                pago.Id_Contrato = id;
                pago.Monto = user.Inmueble.Precio;
                pago.Fecha = DateTime.Now;
                repoPago.Alta(pago);
                return View("/Pago/Index/");
            }
            
        }
        public ActionResult PorFechas(){


            return View();
        }
        [HttpPost]
        public ActionResult PorFechas(Contrato c){
            IList<Inmueble> inmuebles = new List<Inmueble>() ;
            var aux = repo.ObtenerPorFechas(c.FechaInicio, c.FechaExpiracion);
            foreach (var item in aux)
            {
                var x = repoInmueble.ObtenerPorId(item.Id_Inquilino);
                inmuebles.Add(x);
            }


            return View("/Inmueble/Index", inmuebles);
        }*/
    }
}