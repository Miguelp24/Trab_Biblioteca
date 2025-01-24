using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryLab.Data;
using LibraryLab.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace LibraryLab.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TabelaBibliosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TabelaBibliosController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _webHostEnvironment = environment;
        }

        // GET: TabelaBiblios
        public async Task<IActionResult> Index()
        {
            return View(await _context.TabelaBiblio.ToListAsync());
        }

        // GET: TabelaBiblios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabelaBiblio = await _context.TabelaBiblio
                .FirstOrDefaultAsync(m => m.Name == id);
            if (tabelaBiblio == null)
            {
                return NotFound();
            }

            return View(tabelaBiblio);
        }

        // GET: TabelaBiblios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TabelaBiblios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Localizacao,Email,Telefone,Horario_abertura,Horario_fecho,Image")] BiblioViewModel tabelaBiblio)
        {
            var PhotoExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

            var extension = Path.GetExtension(tabelaBiblio.Image.FileName).ToLower();

            if (!PhotoExtensions.Contains(extension))
            {
                ModelState.AddModelError("CoverPhoto", "Please, submit a valid image (jpg, jpeg, png, gif, bmp).");
            }

            if (ModelState.IsValid)
            {
                var newBiblio = new TabelaBiblio(); //create a new Biblio and populate whit fields of the biblio parameter
                newBiblio.Name = tabelaBiblio.Name;
                newBiblio.Localizacao = tabelaBiblio.Localizacao;
                newBiblio.Email = tabelaBiblio.Email;
                newBiblio.Telefone = tabelaBiblio.Telefone;
                newBiblio.Horario_abertura = tabelaBiblio.Horario_abertura;
                newBiblio.Horario_fecho = tabelaBiblio.Horario_fecho;
                newBiblio.Image = tabelaBiblio.Image.FileName;

                //save the files in the corresponding folder
                // Save the Image file in the Cover folder
                string coverFileName = Path.GetFileName(tabelaBiblio.Image.FileName);
                string coverFullPath = Path.Combine(_webHostEnvironment.WebRootPath, "ImagemBiblio", coverFileName);

                using (var stream = new FileStream(coverFullPath, FileMode.Create))
                {
                    await tabelaBiblio.Image.CopyToAsync(stream);
                }


                // Add the Autor in the database
                _context.Add(newBiblio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));




            }
            return View(tabelaBiblio);
        }

        // GET: TabelaBiblios/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabelaBiblio = await _context.TabelaBiblio.FindAsync(id);
            if (tabelaBiblio == null)
            {
                return NotFound();
            }
            return View(tabelaBiblio);
        }

        // POST: TabelaBiblios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,Localizacao,Email,Telefone,Horario_abertura,Horario_fecho,Image")] TabelaBiblio tabelaBiblio)
        {
            if (id != tabelaBiblio.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tabelaBiblio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TabelaBiblioExists(tabelaBiblio.Name))
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
            return View(tabelaBiblio);
        }

        // GET: TabelaBiblios/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabelaBiblio = await _context.TabelaBiblio
                .FirstOrDefaultAsync(m => m.Name == id);
            if (tabelaBiblio == null)
            {
                return NotFound();
            }

            return View(tabelaBiblio);
        }

        // POST: TabelaBiblios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tabelaBiblio = await _context.TabelaBiblio.FindAsync(id);
            if (tabelaBiblio != null)
            {
                _context.TabelaBiblio.Remove(tabelaBiblio);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TabelaBiblioExists(string id)
        {
            return _context.TabelaBiblio.Any(e => e.Name == id);
        }
    }
}
