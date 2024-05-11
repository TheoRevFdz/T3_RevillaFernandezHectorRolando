using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using T3_RevillaFernandezHectorRolando.Models;
using T3_RevillaFernandezHectorRolando.Util;
using T3_RevillaFernandezHectorRolando.Services;

namespace T3_RevillaFernandezHectorRolando.Controllers
{
    public class InicioController : Controller
    {
        private readonly IEmpleadoService _empleadoService;

        public InicioController(IEmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string clave)
        {
            Empleado usuario_encontrado = await _empleadoService.GetEmpleado(correo, EmpleadoUtil.EncriptarClave(clave));

            if (usuario_encontrado == null) {
                ViewData["Mensaje"] = "No se encontrarion coincidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>() { 
                new Claim(ClaimTypes.Name, usuario_encontrado.nombreCompleto)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index", "Home");
        }
    }
}
