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
    public class PropietarioController : Controller
    {
        RepositorioPropietario repo = new RepositorioPropietario();
        RepositorioInquilino repoInquilino = new RepositorioInquilino();
        
        
        [Authorize]
        public ActionResult Index()
        {
            var lista = repo.ObtenerTodos();
            
            return View(lista);
        }
        [Authorize]
        public ActionResult Details(int id)
        {
            var aux = repo.ObtenerPorId(id);
            return View(aux);
        }


        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Create(Propietario i)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                
                var aux = repo.ObtenerTodos();
                var email = repo.ObtenerPorEmail(i.Email);
                var telefono = repo.ObtenerPorTelefono(i.Telefono);
                var dni = repo.ObtenerPorDNI(i.Dni);
                var inquilinoEmail = repoInquilino.ObtenerPorEmail(i.Email);
                var inquilinoTelefono = repoInquilino.ObtenerPorTelefono(i.Telefono);
                var inquilinoDni = repoInquilino.ObtenerPorDNI(i.Dni);
                
                if(Regex.IsMatch(i.Nombre, @"^[a-zA-Z]+$") && Regex.IsMatch(i.Apellido, @"^[a-zA-Z]+$") && Regex.IsMatch(i.Dni, "[0-9]") && Regex.IsMatch(i.Telefono, "[0-9]")){
                    if(email != null ||telefono != null || dni != null || inquilinoEmail != null ||inquilinoTelefono != null || inquilinoDni != null){
                    ViewBag.Error = "Email, Telefono o Dni ya registrados.";
                    return View("Index", aux);
                    }else{
                        int alta = repo.Alta(i);
                        ViewBag.Mensaje = "¡Propietario Creado exitosamente!";
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
           Propietario i = null;
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
        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propietario entidad)
        {
            try
            {
                var aux = repo.ObtenerTodos();
                repo.Baja(id);
                ViewBag.Mensaje ="Propietario eliminado";
                return View("Index", aux);
            }
            catch
            {
                
                throw;
            }
        }
    }
}