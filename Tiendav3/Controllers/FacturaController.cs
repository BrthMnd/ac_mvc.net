using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tiendav3.Models;

namespace Tiendav3.Controllers
{
    public class FacturaController : Controller
    {
        private readonly crud_meloContext _context;

        public FacturaController(crud_meloContext context)
        {
            _context = context;
        }

        // GET: Factura
        public async Task<IActionResult> Index()
        {
            var crud_meloContext = _context.Facturas.Include(f => f.ClienteCedulaNavigation).Include(f => f.ProductoCodigoNavigation);
            return View(await crud_meloContext.ToListAsync());
        }

        // GET: Factura/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas
                .Include(f => f.ClienteCedulaNavigation)
                .Include(f => f.ProductoCodigoNavigation)
                .FirstOrDefaultAsync(m => m.NumeroFactura == id);
            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        // GET: Factura/Create
        public IActionResult Create()
        {
            ViewData["ClienteCedula"] = new SelectList(_context.Clientes, "Cedula", "Cedula");
            ViewData["ProductoCodigo"] = new SelectList(_context.Productos, "Codigo", "Codigo");
            return View();
        }

        // POST: Factura/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumeroFactura,ClienteCedula,ValorTotal,Fecha,ProductoCodigo,Cantidad,Valor")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                var producto = await _context.Productos.FindAsync(factura.ProductoCodigo);
                if (producto == null)
                {
                    return NotFound();
                }

                if (factura.Cantidad > producto.Cantidad)
                {
                    ModelState.AddModelError("Cantidad", "La cantidad de productos no es válida.");
                    ViewData["ClienteCedula"] = new SelectList(_context.Clientes, "Cedula", "Cedula", factura.ClienteCedula);
                    ViewData["ProductoCodigo"] = new SelectList(_context.Productos, "Codigo", "Codigo", factura.ProductoCodigo);
                    return View(factura);
                }
                producto.Cantidad = factura.Cantidad;
                producto.CalcularValorTotal();

                factura.Valor = producto.Precio;

                factura.ValorTotal = factura.Cantidad * factura.Valor;


                _context.Add(factura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteCedula"] = new SelectList(_context.Clientes, "Cedula", "Cedula", factura.ClienteCedula);
            ViewData["ProductoCodigo"] = new SelectList(_context.Productos, "Codigo", "Codigo", factura.ProductoCodigo);
            return View(factura);
        }

        // GET: Factura/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
            {
                return NotFound();
            }

            ViewData["ClienteCedula"] = new SelectList(_context.Clientes, "Cedula", "Cedula", factura.ClienteCedula);
            ViewData["ProductoCodigo"] = new SelectList(_context.Productos, "Codigo", "Codigo", factura.ProductoCodigo);
            return View(factura);
        }

        // POST: Factura/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumeroFactura,ClienteCedula,ValorTotal,Fecha,ProductoCodigo,Cantidad,Valor")] Factura factura)
        {
            if (id != factura.NumeroFactura)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var producto = await _context.Productos.FindAsync(factura.ProductoCodigo);
                if (producto == null)
                {
                    return NotFound();
                }

                if (factura.Cantidad > producto.Cantidad)
                {
                    ModelState.AddModelError("Cantidad", "La cantidad de productos no es válida.");
                    ViewData["ClienteCedula"] = new SelectList(_context.Clientes, "Cedula", "Cedula", factura.ClienteCedula);
                    ViewData["ProductoCodigo"] = new SelectList(_context.Productos, "Codigo", "Codigo", factura.ProductoCodigo);
                    return View(factura);
                }
                producto.Cantidad = factura.Cantidad;
                producto.CalcularValorTotal();

                factura.Valor = producto.Precio;

                factura.ValorTotal = factura.Cantidad * factura.Valor;
                try
                {
                    _context.Update(factura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacturaExists(factura.NumeroFactura))
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
            ViewData["ClienteCedula"] = new SelectList(_context.Clientes, "Cedula", "Cedula", factura.ClienteCedula);
            ViewData["ProductoCodigo"] = new SelectList(_context.Productos, "Codigo", "Codigo", factura.ProductoCodigo);
            return View(factura);
        }

        // GET: Factura/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas
                .Include(f => f.ClienteCedulaNavigation)
                .Include(f => f.ProductoCodigoNavigation)
                .FirstOrDefaultAsync(m => m.NumeroFactura == id);
            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        // POST: Factura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Facturas == null)
            {
                return Problem("Entity set 'crud_meloContext.Facturas'  is null.");
            }
            var factura = await _context.Facturas.FindAsync(id);
            if (factura != null)
            {
                _context.Facturas.Remove(factura);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacturaExists(int id)
        {
          return (_context.Facturas?.Any(e => e.NumeroFactura == id)).GetValueOrDefault();
        }
    }
}
