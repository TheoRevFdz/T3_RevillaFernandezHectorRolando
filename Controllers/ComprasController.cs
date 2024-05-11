using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using T3_RevillaFernandezHectorRolando.Data;
using T3_RevillaFernandezHectorRolando.Models;
using T3_RevillaFernandezHectorRolando.Util;

namespace T3_RevillaFernandezHectorRolando.Controllers
{
    public class ComprasController : Controller
    {
        private readonly T3_RevillaFernandezHectorRolandoContext _context;

        public ComprasController(T3_RevillaFernandezHectorRolandoContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index(
            string searchNombre,
            DateTime? fecCompra,
            string? currentFilterNombreProducto,
            DateTime? currentFilterFechaCompra,
            string sortOrder,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NombreProductoSort"] = String.IsNullOrEmpty(sortOrder) ? "nomProd_desc" : "";
            ViewData["FechaCompraSort"] = sortOrder == "fecCompra" ? "fecCompra_desc" : "fecCompra";
            ViewData["CantidadSort"] = sortOrder == "cantidad" ? "cantidad_desc" : "cantidad";

            if (searchNombre != null || fecCompra != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchNombre = currentFilterNombreProducto;
                fecCompra = currentFilterFechaCompra;
            }

            ViewData["CurrentFilterNombreProducto"] = searchNombre;
            ViewData["CurrentFilterFechaCompra"] = fecCompra;

            var compra = from c in _context.Compra
                         select c;

            switch (sortOrder)
            {
                case "nomProd_desc":
                    compra = compra.OrderByDescending(c => c.nombreProducto);
                    break;
                case "fecCompra_desc":
                    compra = compra.OrderByDescending(c => c.fechaCompra);
                    break;
                case "cantidad_desc":
                    compra = compra.OrderByDescending(c => c.cantidad);
                    break;
                case "fecCompra":
                    compra = compra.OrderBy(c => c.fechaCompra);
                    break;
                case "cantidad":
                    compra = compra.OrderBy(c => c.cantidad);
                    break;
                default:
                    compra = compra.OrderBy(c => c.nombreProducto);
                    break;
            }

            if (!String.IsNullOrEmpty(searchNombre))
            {
                compra = compra.Where(c => c.nombreProducto!.Contains(searchNombre));
            }
            if (fecCompra != null)
            {
                compra = compra.Where(c => c.fechaCompra.Year == fecCompra.Value.Year
                && c.fechaCompra.Month == fecCompra.Value.Month
                && c.fechaCompra.Day == fecCompra.Value.Day);
            }
            int pageSize = 5;

            return View(await PaginatedListUtil<Compra>.CreateAsync(compra.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra
                .FirstOrDefaultAsync(m => m.id == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // GET: Compras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nombreProducto,fechaCompra,cantidad,precioUnitario")] Compra compra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(compra);
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nombreProducto,fechaCompra,cantidad,precioUnitario")] Compra compra)
        {
            if (id != compra.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.id))
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
            return View(compra);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra
                .FirstOrDefaultAsync(m => m.id == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compra = await _context.Compra.FindAsync(id);
            if (compra != null)
            {
                _context.Compra.Remove(compra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
            return _context.Compra.Any(e => e.id == id);
        }
    }
}
