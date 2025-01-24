using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryLab.Data;
using LibraryLab.Models;
using Microsoft.AspNetCore.Authorization;

namespace LibraryLab.Controllers
{
    [Authorize(Roles = "Bibliotecario")]
    public class AutorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AutorController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _webHostEnvironment = environment;
        }

        // GET: Autor
        public async Task<IActionResult> Index()
        {
            return View(await _context.Autor.ToListAsync());
        }

        // GET: Autor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // GET: Autor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Biography,Image")] AutorViewModel autor)
        {
            var PhotoExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

            var extension = Path.GetExtension(autor.Image.FileName).ToLower();

            if (!PhotoExtensions.Contains(extension))
            {
                ModelState.AddModelError("CoverPhoto", "Please, submit a valid image (jpg, jpeg, png, gif, bmp).");
            }
         
            if (ModelState.IsValid)
            {
                var newAutor = new Autor(); //create a new Autor and populate whit fields of the autor parameter
                newAutor.Name = autor.Name;
                newAutor.Biography = autor.Biography;
                newAutor.Image = autor.Image.FileName;

                //save the files in the corresponding folder
                // Save the Image file in the Cover folder
                string coverFileName = Path.GetFileName(autor.Image.FileName);
                string coverFullPath = Path.Combine(_webHostEnvironment.WebRootPath, "ImagemAutor", coverFileName);

                using (var stream = new FileStream(coverFullPath, FileMode.Create))
                {
                    await autor.Image.CopyToAsync(stream);
                }


                // Add the Autor in the database
                _context.Add(newAutor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));




            }
            return View(autor);
        }

        // GET: Autor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // POST: Autor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Biography,Image")] Autor autor)
        {
            if (id != autor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.Id))
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
            return View(autor);
        }

        // GET: Autor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autor
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // POST: Autor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autor = await _context.Autor.FindAsync(id);
            if (autor != null)
            {
                _context.Autor.Remove(autor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
            return _context.Autor.Any(e => e.Id == id);
        }
    }
}
