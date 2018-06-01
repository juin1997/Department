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
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Activities
        public async Task<IActionResult> Index(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.id = id;
            return View(await _context.Activities.Where(a => a.DepartID == id).ToListAsync());
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .SingleOrDefaultAsync(m => m.ID == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create(long id)
        {
            ViewBag.id = id;
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(long id, [Bind("Name,Actaddress,Acttime,Actintroduction,Actpictures,Enabled")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                activity.DepartID = id;
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id });
            }
            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.SingleOrDefaultAsync(m => m.ID == id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name,DepartID,Actaddress,Acttime,Actintroduction,Actpictures,Enabled")] Activity activity)
        {
            if (id != activity.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                id = activity.DepartID;
                return RedirectToAction("Index",new { id });
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .SingleOrDefaultAsync(m => m.ID == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var activity = await _context.Activities.SingleOrDefaultAsync(m => m.ID == id);
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            id = activity.DepartID;
            return RedirectToAction("Index", new { id });
        }

        private bool ActivityExists(long id)
        {
            return _context.Activities.Any(e => e.ID == id);
        }
    }
}
