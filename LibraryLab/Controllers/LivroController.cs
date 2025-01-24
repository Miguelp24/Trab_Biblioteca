using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryLab.Data;
using LibraryLab.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace LibraryLab.Controllers
{
    [Authorize(Roles = "Bibliotecario")]
    public class LivroController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LivroController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _webHostEnvironment = environment;
        }

        // GET: Livro
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Livro.Include(l => l.Autor).Include(l => l.Genero);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Livro/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livro
                .Include(l => l.Autor)
                .Include(l => l.Genero)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // GET: Livro/Create
        public IActionResult Create()
        {
            ViewData["AutorId"] = new SelectList(_context.Autor, "Id", "Name");
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "Name");
            ViewData["Estado"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Disponivel", Value = "True"},
                new SelectListItem { Text = "Indisponivel", Value = "False"}
            }, "Value", "Text");
            return View();
        }

        // POST: Livro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ISBN,Titulo,AutorId,GeneroId,Num_exemplares,Image,Sinopse,Estado")] LivroViewModel livro)
        {
            var PhotoExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

            var extension = Path.GetExtension(livro.Image.FileName).ToLower();

            if (!PhotoExtensions.Contains(extension))
            {
                ModelState.AddModelError("CoverPhoto", "Please, submit a valid image (jpg, jpeg, png, gif, bmp).");
            }

            if (ModelState.IsValid)
            {
                var newLivro = new Livro(); //create a new Livro and populate whit fields of the livro parameter
                newLivro.ISBN = livro.ISBN;
                newLivro.Titulo = livro.Titulo;
                newLivro.AutorId = livro.AutorId;
                newLivro.GeneroId = livro.GeneroId;
                newLivro.Num_exemplares = livro.Num_exemplares;
                newLivro.Image = livro.Image.FileName;
                newLivro.Sinopse = livro.Sinopse;
                newLivro.Estado = livro.Estado;



                //save the files in the corresponding folder
                // Save the Image file in the Cover folder
                string coverFileName = Path.GetFileName(livro.Image.FileName);
                string coverFullPath = Path.Combine(_webHostEnvironment.WebRootPath, "ImagemLivro", coverFileName);

                using (var stream = new FileStream(coverFullPath, FileMode.Create))
                {
                    await livro.Image.CopyToAsync(stream);
                }


                // Add the Livro in the database
                _context.Add(newLivro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));




            }
            return View(livro);
        }

        // GET: Livro/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livro.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }
            ViewData["AutorId"] = new SelectList(_context.Autor, "Id", "Name", livro.AutorId);
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "Name", livro.GeneroId);
            ViewData["Estado"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Disponivel", Value = "True"},
                new SelectListItem { Text = "Indisponivel", Value = "False"}
            }, "Value", "Text");
            
            return View(livro);
        }

        // POST: Livro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ISBN,Titulo,AutorId,GeneroId,Num_exemplares,Image,Sinopse,Estado")] Livro livro)
        {
            if (id != livro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivroExists(livro.Id))
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
            ViewData["AutorId"] = new SelectList(_context.Autor, "Id", "Name", livro.AutorId);
            ViewData["GeneroId"] = new SelectList(_context.Genero, "Id", "Name", livro.GeneroId);
            ViewData["Estado"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Text = "Disponivel", Value = "True"},
                new SelectListItem { Text = "Indisponivel", Value = "False"}
            }, "Value", "Text");
            return View(livro);
        }

        // GET: Livro/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livro
                .Include(l => l.Autor)
                .Include(l => l.Genero)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // POST: Livro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livro = await _context.Livro.FindAsync(id);
            if (livro != null)
            {
                _context.Livro.Remove(livro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LivroExists(int id)
        {
            return _context.Livro.Any(e => e.Id == id);
        }
    }
}
