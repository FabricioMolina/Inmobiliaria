using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MolinaInmobilaria.Models;
using MolinaInmobilaria.Repositorios;

namespace MolinaInmobilaria.Controllers
{
    public class InquilinoController : Controller
    {
        RepositorioInquilino repo = new RepositorioInquilino();
        RepositorioPropietario repoPropietario = new RepositorioPropietario();
        
       
         [Authorize]
        public ActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            
            return View(lista);
        }


        
         [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [Authorize]
        public ActionResult Create(Inquilino i)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                var aux = repo.ObtenerTodos();
                var email = repo.ObtenerPorEmail(i.Email);
                var telefono = repo.ObtenerPorTelefono(i.Telefono);
                var dni = repo.ObtenerPorDNI(i.Dni);
                var inquilinoEmail = repoPropietario.ObtenerPorEmail(i.Email);
                var inquilinoTelefono = repoPropietario.ObtenerPorTelefono(i.Telefono);
                var inquilinoDni = repoPropietario.ObtenerPorDNI(i.Dni);
                
                if(Regex.IsMatch(i.Nombre, @"^[a-zA-Z]+$") && Regex.IsMatch(i.Apellido, @"^[a-zA-Z]+$") && Regex.IsMatch(i.Dni, "[0-9]") && Regex.IsMatch(i.Telefono, "[0-9]")){
                    if(email != null ||telefono != null || dni != null || inquilinoEmail != null ||inquilinoTelefono != null || inquilinoDni != null){
                    ViewBag.Error = "Email, Telefono o Dni ya registrados.";
                    return View("Index", aux);
                    }else{
                        int alta = repo.Alta(i);
                        ViewBag.Mensaje = "Inquilino Creado exitosamente!";
                        var nuevoaux = repo.ObtenerTodos();
                        return View("Index", nuevoaux);
                }
            }else{
                ViewBag.Error = "Los nombres no deben contener un número y/o Tanto como el DNI como el teléfono solo puede estar formado por números.";
                return View("Create");
            }
            }
            catch
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
                
                return View(aux);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
        [HttpPost]
         [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
           Inquilino i = null;
            var aux = repo.ObtenerTodos();
            try
            {
                i = repo.ObtenerPorId(id);
                i.Nombre = collection["Nombre"];
                i.Apellido = collection["Apellido"];
                i.Dni = collection["Dni"];
                i.Email = collection["Email"];
                i.Telefono = collection["Telefono"];
                repo.Modificacion(i);
                
                ViewBag.Mensaje = "Datos guardados correctamente";
                return View("Index", aux);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Authorize]
        public ActionResult Details(int id)
        {
            var aux = repo.ObtenerPorId(id);
            return View(aux);
        }

        
        [Authorize(Policy = "Admin")]
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

        
        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino entidad)
        {
            var aux = repo.ObtenerTodos();
            try
            {
                
                repo.Baja(id);
                ViewBag.Mensaje = "Inquilino Eliminado";
                return View("Index", aux);
            }
            catch
            {
                
               throw;
            }
        }
    }
}