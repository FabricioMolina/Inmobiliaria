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
    public class PagoController : Controller
    {
        PagoRepositorio repo = new PagoRepositorio();
        ContratoRepositorio repoCon = new ContratoRepositorio();
        

        [Authorize]
        public ActionResult Index()
        {
            
            var list = repo.ObtenerTodos();
            return View(list);
        }
        
        [Authorize]
        public ActionResult Create(int id)
        {   

            IList<Pago> repetidos  = repo.ObtenerRepetidos(id);
            var cantidad =  repetidos.Count();
            Contrato contrato = null;
            if(cantidad == 0){
                ViewBag.Cantidad = 1;   
            }else{
                ViewBag.Cantidad = cantidad +1;
            }
            contrato = repoCon.ObtenerPorId(id);
            
            ViewBag.Contrato = contrato;
            
            return View();
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Pago p)
        {
            
            if(!ModelState.IsValid){
                return View();
            }else{
                try
                    {
                        var alta = repo.Alta(p);
                        ViewBag.Mensaje = "Pago creado perfectamente.";
                        return RedirectToAction(nameof(Index));
                    }
                catch(Exception ex)
                    {
                        throw;
                    }
                }
        }
        

        // GET: Pago/Edit/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var user = repo.ObtenerPorId(id);
            return View(user);
        }
        
        [Authorize]
        public ActionResult Edit(int id)
        {
            var aux = repo.ObtenerPorId(id);
            return View(aux);
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago pagon)
        {
            Pago i = null;
            try
            {
                
                i = repo.ObtenerPorId(id);
                i.Id_Contrato = pagon.Id_Contrato;
                i.Monto = pagon.Monto;
                i.Fecha = pagon.Fecha;
                repo.Modificacion(i);
                
                ViewBag.Mensaje  = "Datos guardados correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: Pago/Delete/5
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var aux = repo.ObtenerPorId(id);
            return View(aux);
        }

        // POST: Pago/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var aux = repo.ObtenerPorId(id);
                repo.Baja(id);
                ViewBag.Mensaje = "Pago Eliminado perfectamente.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}