using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Security.DataProtection.WebUI.Models;

namespace Security.DataProtection.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly NLayerDBContext _context;
        private readonly IDataProtector _dataProtector;
        private readonly IDataProtector _dataProtectorImage;
        public ProductsController(NLayerDBContext context, IDataProtectionProvider dataProtectionProvider)
        {
            _context = context;
            _dataProtector = dataProtectionProvider.CreateProtector(Constants.DataProtection.PRODUCTDATAPROTECTION);
            _dataProtectorImage = dataProtectionProvider.CreateProtector(Constants.DataProtection.PRODUCTDATAPROTECTION_IMAGE);
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            products.ForEach(x =>
            {
                x.EncryptedId = _dataProtector.Protect(x.Id.ToString());
            });

            return _context.Products != null ?
                        View(products) :
                        Problem("Entity set 'NLayerDBContext.Products'  is null.");
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            int decrptedId = int.Parse(_dataProtector.Unprotect(id));
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == decrptedId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Stock,Price,CategoryId,CreatedDate,UpdatedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            int decrptedId = int.Parse(_dataProtector.Unprotect(id));

            var product = await _context.Products.FindAsync(decrptedId);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Stock,Price,CategoryId,CreatedDate,UpdatedDate")] Product product)
        {
            int decrptedId = int.Parse(_dataProtector.Unprotect(id));
            if (decrptedId != product.Id)
            {
                return NotFound();
            }



            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            int decrptedId = int.Parse(_dataProtector.Unprotect(id));


            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == decrptedId);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'NLayerDBContext.Products'  is null.");
            }
            int decrptedId = int.Parse(_dataProtector.Unprotect(id));

            var product = await _context.Products.FindAsync(decrptedId);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
