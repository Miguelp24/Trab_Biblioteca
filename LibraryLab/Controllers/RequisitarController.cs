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
    public class RequisitarController : Controller
    {

        private readonly ApplicationDbContext _context;

        public RequisitarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Requisitar
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Requisitar.Include(r => r.Bibliotecario).Include(r => r.Leitor).Include(r => r.Livro);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Requisitar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisitar = await _context.Requisitar
                .Include(r => r.Bibliotecario)
                .Include(r => r.BibliotecarioRecebe)
                .Include(r => r.Leitor)
                .Include(r => r.Livro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requisitar == null)
            {
                return NotFound();
            }

            return View(requisitar);
        }

        // GET: Requisitar/Create
        public IActionResult Create()
        {
            ViewData["BibliotecarioId"] = new SelectList(_context.Utilizadores, "Id", "Id");
            ViewData["LeitorId"] = new SelectList(_context.Utilizadores, "Id", "Id");
            ViewData["LivroId"] = new SelectList(_context.Livro, "Id", "ISBN");
            return View();
        }

        // POST: Requisitar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BibliotecarioId,LeitorId,LivroId,DataRequisicao,DataEmprestimo,DataDevolucao")] Requisitar requisitar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requisitar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BibliotecarioId"] = new SelectList(_context.Utilizadores, "Id", "Id", requisitar.BibliotecarioId);
            ViewData["LeitorId"] = new SelectList(_context.Utilizadores, "Id", "Id", requisitar.LeitorId);
            ViewData["LivroId"] = new SelectList(_context.Livro, "Id", "ISBN", requisitar.LivroId);
            return View(requisitar);
        }

        // GET: Requisitar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisitar = await _context.Requisitar.FindAsync(id);
            if (requisitar == null)
            {
                return NotFound();
            }
            ViewData["BibliotecarioId"] = new SelectList(_context.Utilizadores, "Id", "Id", requisitar.BibliotecarioId);
            ViewData["LeitorId"] = new SelectList(_context.Utilizadores, "Id", "Id", requisitar.LeitorId);
            ViewData["LivroId"] = new SelectList(_context.Livro, "Id", "ISBN", requisitar.LivroId);
            return View(requisitar);
        }

        // POST: Requisitar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BibliotecarioId,LeitorId,LivroId,DataRequisicao,DataEmprestimo,DataDevolucao")] Requisitar requisitar)
        {
            if (id != requisitar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requisitar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequisitarExists(requisitar.Id))
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
            ViewData["BibliotecarioId"] = new SelectList(_context.Utilizadores, "Id", "Id", requisitar.BibliotecarioId);
            ViewData["LeitorId"] = new SelectList(_context.Utilizadores, "Id", "Id", requisitar.LeitorId);
            ViewData["LivroId"] = new SelectList(_context.Livro, "Id", "ISBN", requisitar.LivroId);
            return View(requisitar);
        }

        // GET: Requisitar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requisitar = await _context.Requisitar
                .Include(r => r.Bibliotecario)
                .Include(r => r.Leitor)
                .Include(r => r.Livro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requisitar == null)
            {
                return NotFound();
            }

            return View(requisitar);
        }

        // POST: Requisitar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requisitar = await _context.Requisitar.FindAsync(id);
            if (requisitar != null)
            {
                _context.Requisitar.Remove(requisitar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequisitarExists(int id)
        {
            return _context.Requisitar.Any(e => e.Id == id);
        }



        [Authorize(Roles = "Bibliotecario")]
        public async Task<IActionResult> Emprestimo(int id)
        {
            var requisicao = await _context.Requisitar.FindAsync(id);
            if (requisicao == null)
            {
                return NotFound();
            }

            var userName = User.Identity?.Name;
            if (userName == null)
            {
                return Unauthorized();
            }

            var bibliotecario = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Username == userName);
            if (bibliotecario == null)
            {
                return NotFound("Bibliotecário não encontrado.");
            }

            requisicao.BibliotecarioId = bibliotecario.Id;
            requisicao.DataEmprestimo = DateTime.Now;

            _context.Update(requisicao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Bibliotecario")]
        public async Task<IActionResult> Devolucao(int id)
        {
            var requisicao = await _context.Requisitar.FindAsync(id);
            if (requisicao == null)
            {
                return NotFound();
            }

            var userName = User.Identity?.Name;
            if (userName == null)
            {
                return Unauthorized();
            }

            var bibliotecario = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Username == userName);
            if (bibliotecario == null)
            {
                return NotFound("Bibliotecário não encontrado.");
            }

            requisicao.BibliotecarioRecebeId = bibliotecario.Id;
            requisicao.DataDevolucao = DateTime.Now;

            var livro = await _context.Livro.FirstOrDefaultAsync(l => l.Id == requisicao.LivroId);

            livro.Num_exemplares++;


            _context.Update(requisicao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
