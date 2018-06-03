using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Department.Data;
using Department.Models;

namespace Department.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Applications
        public async Task<IActionResult> Index(long? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            ViewBag.id = id;
            return View(await _context.Applications.Where(a => a.DepartID == id).ToListAsync());
        }

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .SingleOrDefaultAsync(m => m.ID == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: Applications/Create
        public IActionResult Create(long id)
        {
            ViewBag.id = id;
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(long id, [Bind("Count,Time,Grade,Institute,Address,Enabled")] Application application)
        {
            if (ModelState.IsValid)
            {
                application.DepartID = id;
                application.DName = _context.Departs.Where(d => d.ID == id).FirstOrDefault().Name;
                _context.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id });
            }
            return View(application);
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications.SingleOrDefaultAsync(m => m.ID == id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,DepartID,DName,Count,Time,Grade,Institute,Address,Enabled")] Application application)
        {
            if (id != application.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(application);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                id = application.DepartID;
                return RedirectToAction("Index", new { id });
            }
            return View(application);
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .SingleOrDefaultAsync(m => m.ID == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var application = await _context.Applications.SingleOrDefaultAsync(m => m.ID == id);
            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
            id = application.DepartID;
            return RedirectToAction("Index", new { id });
        }

        private bool ApplicationExists(long id)
        {
            return _context.Applications.Any(e => e.ID == id);
        }
    }
}
