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
using Microsoft.AspNetCore.Identity;



namespace LibraryLab.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UtilizadoresController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;


        private readonly ApplicationDbContext _context;


        public UtilizadoresController(ApplicationDbContext context, UserManager<IdentityUser> userManager, ILogger<RegisterModel> logger, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
            _roleManager = roleManager;
        }

        // GET: Utilizadores
        public async Task<IActionResult> Index()
        {
            var utilizadores = await _context.Utilizadores.ToListAsync();
            var inativosCount = utilizadores.Count(u => !u.IsActivated);
            ViewBag.InativosCount = inativosCount;
            return View(utilizadores);
        }

        // GET: Utilizadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }



        // GET: Utilizadores/Create
        public IActionResult Create()
        {
            ViewBag.roles = _roleManager.Roles.ToList();
            return View();
           
        }

        // POST: Utilizadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Username,Password,Email,Role")] Utilizador utilizador, RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                
                await _context.SaveChangesAsync();
                var user = new IdentityUser { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, model.Role);

                    Utilizador newPerfil = new Utilizador { Nome = model.Nome, Username = user.UserName, Password = user.PasswordHash, Email = user.Email, Role = model.Role, IsActivated=true };

                    _context.Utilizadores.Add(newPerfil);
                    _context.SaveChanges();

                    var userId = await _userManager.GetUserIdAsync(user);
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                
                

            }
            return View(utilizador);
        }

        // GET: Utilizadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadores.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }
            return View(utilizador);
        }

        // POST: Utilizadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Username,Password,Email,Role")] Utilizador utilizador)
        {
            if (id != utilizador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilizador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadorExists(utilizador.Id))
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
            return View(utilizador);
        }

        // GET: Utilizadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }

        // POST: Utilizadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var utilizador = await _context.Utilizadores.FindAsync(id);
            if (utilizador != null)
            {
                _context.Utilizadores.Remove(utilizador);
            }


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadorExists(int id)
        {
            return _context.Utilizadores.Any(e => e.Id == id);
        }




        // GET: Utilizadores/Block/5
        public async Task<IActionResult> Block(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadores.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }

        // POST: Utilizadores/Block/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Block(int id, [Bind("Id,BlockReason")] Utilizador utilizador)
        {
            var user = await _context.Utilizadores.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                user.IsBlocked = true;
                user.BlockReason = utilizador.BlockReason;

                _context.Update(user);
                await _context.SaveChangesAsync();

                // Opcional: Bloquear o usuário no Identity
                var identityUser = await _userManager.FindByIdAsync(user.Id.ToString());
                if (identityUser != null)
                {
                    await _userManager.SetLockoutEndDateAsync(identityUser, DateTimeOffset.MaxValue);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(utilizador);
        }
        // GET: Utilizadores/Unblock/5
        public async Task<IActionResult> Unblock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Utilizadores.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }

        // POST: Utilizadores/Unblock/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unblock(int id)
        {
            var user = await _context.Utilizadores.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                user.IsBlocked = false;
                user.BlockReason = null;

                _context.Update(user);
                await _context.SaveChangesAsync();

                // Opcional: Desbloquear o usuário no Identity
                var identityUser = await _userManager.FindByIdAsync(user.Id.ToString());
                if (identityUser != null)
                {
                    await _userManager.SetLockoutEndDateAsync(identityUser, null);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: Utilizadores/Activate/5
        public async Task<IActionResult> Activate (Utilizador utilizador)
        {

            var inativos = await _context.Utilizadores.Where(u => !u.IsActivated).ToListAsync();
            return View(inativos);
        }


        // POST: Utilizadores/Activate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateConfirmed(int id)
        {
            var utilizador = await _context.Utilizadores.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }

            utilizador.IsActivated = true;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilizador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadorExists(utilizador.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                var inativos = await _context.Utilizadores.Where(u => !u.IsActivated).ToListAsync();
                return PartialView("TabelaAtivar", inativos);
            }
            return PartialView("TabelaAtivar", new List<Utilizador> { utilizador });
        }

        }
    }


