using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MolinaInmobilaria.Repositorios;
using MolinaInmobilaria.Models;
using Microsoft.AspNetCore.Authorization;

namespace MolinaInmobilaria.Controllers
{
    public class InmuebleController : Controller
    {
        InmuebleRepositorio repo = new InmuebleRepositorio();
        RepositorioPropietario repoPro = new RepositorioPropietario();
        ContratoRepositorio repoCon = new ContratoRepositorio();
        [Authorize]
        public ActionResult Index()
        {
            var list = repo.ObtenerTodos();
            return View(list);
        }
        [Authorize]
        public ActionResult Disponibles()
        {
            var list = repo.ObtenerDisponibles();
            return View(list);
        }
        [Authorize]
        public ActionResult PorPropietario(int id)
        {
            var list = repo.ObtenerPorPropietario(id);
            return View(list);
        }

        [Authorize]
        public ActionResult Create()
        {
            var list = repoPro.ObtenerTodos();
            ViewBag.propietario = list;
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble i)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                int alta = repo.Alta(i);
                ViewBag.Mensaje = "Inmueble creado perfectamente.";
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
             try
			{
                var aux = repo.ObtenerPorId(id);
                ViewBag.Propietario = repoPro.ObtenerTodos();
                return View(aux);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inmueble i = null;
            try
            {
                i = repo.ObtenerPorId(id);
                i.Id_Propietario = Int32.Parse(collection["Id_Propietario"]);
                i.Direccion = collection["Direccion"];
                i.Latitud = collection["Latitud"];
                i.Longitud = collection["Longitud"];
                i.Precio = float.Parse(collection["Precio"]);
                i.Ambientes = collection["Ambientes"];
                i.Estado = Int32.Parse(collection["Estado"]);
                repo.Modificacion(i);
                ViewBag.Mensaje  = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

         [Authorize(Policy="Admin")]

        public ActionResult Delete(int id)
        {
            try
			{
                var aux = repo.ObtenerPorId(id);
            
                return View(aux);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize(Policy="Admin")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
           try
            {
                // TODO: Add delete logic here
                repo.Baja(id);
                ViewBag.Mensaje  = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                
                return View();
            }
        }
        [Authorize]
        public ActionResult Details(int id)
        {
            var inmueble = repo.ObtenerPorId(id);
            return View(inmueble);
        }
        [Authorize]
        public ActionResult PorFechas(){

            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
         public ActionResult PorFechas(Contrato c){
            var disponibles = repoCon.ObtenerInmueblesNoOcupados(c.FechaInicio.ToString("yyyy-MM-dd"), c.FechaExpiracion.ToString("yyyy-MM-dd"));
            //var disponibles = repoCon.ObtenerInmueblesNoOcupados(c.FechaInicio, c.FechaExpiracion);           
            List<Inmueble> inmuebles = new List<Inmueble>();
            
            var todos = repo.ObtenerTodos();
            if(disponibles.Count() > 0){
                foreach (var item in disponibles)
                    {
                        var aux = repo.ObtenerPorId(item.Id_Inmueble);
                        Inmueble inmueble = aux;
                        inmuebles.Add(inmueble);
                    }
                    ViewBag.Data = "Estos son los Inmuebles disponibles fuera de ese rango de fechas";
                    return View("Index", inmuebles);
            }else{
                ViewBag.Data = "No existen Inmuebles disponibles fuera de ese rango de fechas";
                return View("Index", todos);
            }
            
            
        }
    }
}