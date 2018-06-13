using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Department.Data;
using Department.Models;
using NETCore.MailKit.Core;
using Hangfire;

namespace Department.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _EmailService;

        public ActivitiesController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _EmailService = emailService;
        }

        // GET: Activities
        public async Task<IActionResult> Index(long? id)
        {
            List<Activity> acts = await _context.Activities.ToListAsync();
            foreach (Activity act in acts)
            {
                if (act.Acttime <= DateTime.Now)
                {
                    act.Enabled = false;
                }
            }
            _context.Activities.UpdateRange(acts);
            await _context.SaveChangesAsync();


            ViewBag.id = id;
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
            ViewBag.id = id;
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
        public async Task<IActionResult> Create(long id, [Bind("Name,Actaddress,Acttime,Noticetime,Actintroduction,Enabled")] Activity activity)
        {
            ViewBag.id = id;
            if (ModelState.IsValid)
            {
                activity.DepartID = id;
                _context.Add(activity);
                await _context.SaveChangesAsync();
                if (activity.Noticetime != null)
                {
                    Activity act = await _context.Activities.Where(a => a.Name == activity.Name && a.DepartID == activity.DepartID).FirstOrDefaultAsync();
                    long Aid = act.ID;
                    TimeSpan t = activity.Noticetime - DateTime.Now;
                    BackgroundJob.Schedule(() => SendEmails(id, Aid), t);
                }
                return RedirectToAction("Index", new { id });
            }
            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            ViewBag.id = id;
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
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name,DepartID,Actaddress,Noticetime,Acttime,Actintroduction,Actpictures,Enabled")] Activity activity)
        {
            ViewBag.id = id;
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
            ViewBag.id = id;
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
            ViewBag.id = id;
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

        public async Task<IActionResult> SendEmails(long id, long Aid)
        {
            Activity activity = await _context.Activities.FindAsync(Aid);
            if(activity .Enabled == true)
            {
                string name = activity.Name;
                string address = activity.Actaddress;
                string introduction = activity.Actintroduction;
                string Dname = await _context.Departs.Where(d => d.ID == activity.DepartID).Select(d => d.Name).FirstOrDefaultAsync();
                List<Student> students = new List<Student>();
                List<long> stuids = await _context.DtoMMappings.Where(d => d.DepartID == activity.DepartID).Select(t => t.MemberID).ToListAsync();
                foreach (long stuid in stuids)
                {
                    Student student = await _context.Students.FindAsync(stuid);
                    string email = student.Email;
                    _EmailService.Send(email, $"{Dname}活动通知", $@"活动名称：{name}
活动地点：{address}
具体情况：{introduction}");
                }
            }
            return RedirectToAction("Index", new { id });
        }
    }
}
