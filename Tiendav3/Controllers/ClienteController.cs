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
    public class ClienteController : Controller
    {
        private readonly crud_meloContext _context;

        public ClienteController(crud_meloContext context)
        {
            _context = context;
        }

        // GET: Cliente
        public async Task<IActionResult> Index(string searchCedula)

        {
			var clientes = from c in _context.Clientes
						   select c;

			if (!string.IsNullOrEmpty(searchCedula))
			{
				clientes = clientes.Where(c => c.Cedula.ToString().Contains(searchCedula));
			}

			return View(await clientes.ToListAsync());
			//return _context.Clientes != null ? 
   //                       View(await _context.Clientes.ToListAsync()) :
   //                       Problem("Entity set 'crud_meloContext.Clientes'  is null.");

        }
        // GET: Clientes

        // GET: Cliente/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cedula,Nombre,Direccion,Telefono")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cedula,Nombre,Direccion,Telefono")] Cliente cliente)
        {
            if (id != cliente.Cedula)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Cedula))
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
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clientes == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Cedula == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clientes == null)
            {
                return Problem("Entity set 'crud_meloContext.Clientes' is null.");
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                var facturas = await _context.Facturas.Where(f => f.ClienteCedula == id).ToListAsync();
                _context.Facturas.RemoveRange(facturas);

                _context.Clientes.Remove(cliente);
            }
            else
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
          return (_context.Clientes?.Any(e => e.Cedula == id)).GetValueOrDefault();
        }
    }
}
