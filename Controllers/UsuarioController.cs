using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MolinaInmobilaria.Models;
using MolinaInmobilaria.Repositorios;

namespace MolinaInmobilaria.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private UsuarioRepositorio repo;

        public UsuarioController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
            this.repo = new UsuarioRepositorio(configuration);
        }
        // GET: Usuario
        [Authorize(Policy = "Admin")]
        public ActionResult Index()
        {
            
            var aux = repo.ObtenerTodos();
            return View(aux);
        }

        // GET: Usuario/Details/5
        [Authorize(Policy = "Admin")]
        public ActionResult Details(int id)
        {
            
            var user = repo.ObtenerPorId(id);
            return View(user);
        }
        [Authorize]
        public ActionResult MiPerfil()
        {
            ViewData["Title"] = "Mi perfil";
            var u = repo.ObtenerPorEmail(User.Identity.Name);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View("Details", u);
        }

        [Authorize(Policy = "Admin")]
        public ActionResult Create()
        {
           
            ViewBag.Roles = Usuario.ObtenerRoles();;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin")]
        public ActionResult Create(Usuario user)
        {
            
            if (!ModelState.IsValid)
                return View();
            try
            {
                var aux = repo.ObtenerPorEmail(user.Email);
                if(aux == null){
                    string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: user.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8
                ));
                user.Clave = hash;
                user.Rol = User.IsInRole("Admin") ? user.Rol : (int)enRoles.Empleado;
                var nbreRnd = Guid.NewGuid();
                int res = repo.Alta(user);
                if (user.AvatarFile != null && user.Id > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "img_" + user.Id + ".jpg";
                    string pathCompleto = Path.Combine(path, fileName);
                    user.Avatar = Path.Combine("/Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        user.AvatarFile.CopyTo(stream);
                    }
                    repo.Modificacion(user);
                }
                return RedirectToAction(nameof(Index));    
                }else{
                ViewBag.Error = "Ya existe un Usuario con ese Email";
                return RedirectToAction(nameof(Index)); 
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
            var roles = Usuario.ObtenerRoles();
            ViewBag.Roles = roles;
            var aux = repo.ObtenerPorId(id);
            return View(aux);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Usuario user)
        {
            Usuario u = repo.ObtenerPorId(id);
            try
            {  
                u.Nombre = user.Nombre;
                u.Apellido = user.Apellido;
                u.Email = user.Email;
                if(User.IsInRole("Admin")){
                    u.Rol = user.Rol;
                }
                
                
                
                repo.Modificacion(u);
                return View("Details", u);             
            }
            catch
            {
                throw;
            }
        }
        [Authorize]
        public ActionResult Imagen(int id){
            var aux = repo.ObtenerPorId(id);
            return View(aux);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Imagen(int id, Usuario user){
            
            var u = repo.ObtenerPorId(id);
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            string fileName = "img_" + id + ".jpg";//Path.GetExtension(user.AvatarFile.FileName);
            string pathCompleto = Path.Combine(path, fileName);
            u.Avatar = Path.Combine("/Uploads", fileName);
            System.IO.File.Delete(pathCompleto);
            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        
                        user.AvatarFile.CopyTo(stream);
                    }
            repo.Modificacion(u);                                                            
            return View("Details", u);  
        }
        [Authorize]
        public ActionResult Contraseña(int id){
            var aux = repo.ObtenerPorId(id);
            return View(aux);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contraseña(int id, string claveVieja, string claveNueva, string confirmarClave){
            if (!ModelState.IsValid)
                return View();
            try
            {                 
                var u = repo.ObtenerPorId(id);
                string hashViejo = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: claveVieja,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                string hashNuevo = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: claveNueva,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));                

                string hashConfirmacion = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: confirmarClave,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8
                ));
                if(u.Clave == hashViejo){
                    if(hashNuevo == hashConfirmacion){
                        u.Clave = hashNuevo;
                        repo.Modificacion(u);
                        ViewBag.Mensaje = "La contraseña fue cambiada exitosamente";                
                        return View("Details", u);  
                    }else{
                    ViewBag.Error = "Las contraseñas no coinciden.";
                    return View("Contraseña", u);    
                    }                   
                }else{
                    ViewBag.Error = "Las contraseñas no coinciden.";
                    return View("Contraseña", u);   
                }                                       
            }
            catch
            {
                throw;
            }
        }

       [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var aux = repo.ObtenerPorId(id);
            return View(aux);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Usuario user )
        {
            try
            {
                System.IO.File.Delete(Path.Combine(environment.WebRootPath, "Uploads", "img_" + id + Path.GetExtension(user.Avatar)));
                repo.Baja(id);
                ViewBag.Mensaje = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                
                return View();
            }
        }
        
        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string)? "/Home" : TempData["returnUrl"].ToString();                
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    Usuario user = repo.ObtenerPorEmail(login.Email);
                    var e = user;
                    if (e == null || e.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Email),
                        new Claim("FullName", e.Nombre + " " + e.Apellido),
                        new Claim(ClaimTypes.Role, e.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));
                    TempData.Remove("returnUrl");
                    return Redirect(returnUrl);
                }
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        [Route("salir", Name = "logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuario");
        }



    }
}