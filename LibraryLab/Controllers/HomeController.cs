using LibraryLab.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LibraryLab.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? page, string searchString, int? genreId)
        {
            int currentPage = page ?? 1;
            var bibliotecas = _context.TabelaBiblio.ToList();
            var livrosFiltrados = _context.Livro.Include(l => l.Autor).Include(l => l.Genero).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                livrosFiltrados = livrosFiltrados.Where(l => l.Titulo.Contains(searchString) || (l.Autor != null && l.Autor.Name != null && l.Autor.Name.Contains(searchString)));
            }

            if (genreId.HasValue && genreId.Value > 0)
            {
                livrosFiltrados = livrosFiltrados.Where(l => l.Genero != null && l.Genero.Id == genreId.Value);
            }

            var viewModel = new HomeViewModel
            {
                TabelaBiblio = bibliotecas,
                Livro = livrosFiltrados.ToList(),
                Genres = _context.Genero.ToList()
            };

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentPage"] = currentPage;
            return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var livro = _context.Livro.Include(l => l.Autor).Include(l => l.Genero).FirstOrDefault(l => l.Id == id);
            if (livro == null)
            {
                return NotFound();
            }
            return View(livro);
        }

        [HttpPost]
        [Authorize(Roles = "Leitor")]
        public async Task<IActionResult> Requisitar(int livroId)
        {
            var userName = User.Identity?.Name;
            if (userName == null)
            {
                return Unauthorized();
            }

            var leitor = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Username == userName);

            if (leitor == null)
            {
                return NotFound("Leitor não encontrado.");
            }

            // Verifique se o livro existe 
            var livro = await _context.Livro.FirstOrDefaultAsync(l => l.Id == livroId);
            if (livro == null)
            {
                return NotFound("Livro não encontrado.");
            }

            // Diminua o número de exemplares disponíveis
            livro.Num_exemplares--;

            var requisicao = new Requisitar
            {
                LeitorId = leitor.Id,
                LivroId = livroId,
                DataRequisicao = DateTime.Now
            };

            _context.Requisitar.Add(requisicao);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = livroId });
        }



        [Authorize(Roles = "Leitor")]
        public async Task<IActionResult> Historico()
        {
            var userName = User.Identity?.Name; // Add null check
            if (userName == null)
            {
                return Unauthorized();
            }

            var leitor = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Username == userName);

            if (leitor == null)
            {
                return NotFound("Leitor não encontrado.");
            }

            var requisicoes = await _context.Requisitar
                .Include(r => r.Livro)
                .Where(r => r.LeitorId == leitor.Id)
                .ToListAsync();

            return View(requisicoes);
        }

        public async Task<IActionResult> DetailsHist(int? id)
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

    }
}
