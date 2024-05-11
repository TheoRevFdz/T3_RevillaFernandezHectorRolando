using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using T3_RevillaFernandezHectorRolando.Data;
using T3_RevillaFernandezHectorRolando.Models;
using T3_RevillaFernandezHectorRolando.Util;

namespace T3_RevillaFernandezHectorRolando.Controllers
{
	[Authorize]
	public class EmpleadosController : Controller
    {
        private readonly T3_RevillaFernandezHectorRolandoContext _context;

        public EmpleadosController(T3_RevillaFernandezHectorRolandoContext context)
        {
            _context = context;
        }

        // GET: Empleadoes
        public async Task<IActionResult> Index(
            string searchNombre,
            string email,
            string currentFilterNombreEmp,
            string currentFilterEmail,
            string sortOrder,
            int? pageNumber
            )
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NombreEmpSort"] = String.IsNullOrEmpty(sortOrder) ? "nomEmp_desc" : "";
            ViewData["EmailSort"] = sortOrder == "Email" ? "email_desc" : "Email";

            if (searchNombre != null || email != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchNombre = currentFilterNombreEmp;
                email = currentFilterEmail;
            }

            ViewData["CurrentFilterNombreEmp"] = searchNombre;
            ViewData["CurrentFilterEmail"] = email;

            var empleado = from e in _context.Empleado
                           select e;

            switch (sortOrder)
            {
                case "nomEmp_desc":
                    empleado = empleado.OrderByDescending(e => e.nombreCompleto);
                    break;
                case "email_desc":
                    empleado = empleado.OrderByDescending(e => e.email);
                    break;
                case "Email":
                    empleado = empleado.OrderBy(e => e.email);
                    break;
                default:
                    empleado = empleado.OrderBy(e => e.nombreCompleto);
                    break;
            }

            if (!String.IsNullOrEmpty(searchNombre))
            {
                empleado = empleado.Where(e => e.nombreCompleto!.Contains(searchNombre));
            }
            if (email != null)
            {
                empleado = empleado.Where(e => e.email!.Contains(email));
            }

            int pageSize = 5;

            return View(await PaginatedListUtil<Empleado>.CreateAsync(empleado.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Empleadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleado
                .FirstOrDefaultAsync(m => m.id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleadoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nombreCompleto,email,password")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                empleado.password = EmpleadoUtil.EncriptarClave(empleado.password);
                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        // GET: Empleadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleado.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nombreCompleto,email,password")] Empleado empleado)
        {
            if (id != empleado.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    empleado.password = EmpleadoUtil.EncriptarClave(empleado.password);
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        // GET: Empleadoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleado
                .FirstOrDefaultAsync(m => m.id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleado.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleado.Remove(empleado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleado.Any(e => e.id == id);
        }
    }
}
