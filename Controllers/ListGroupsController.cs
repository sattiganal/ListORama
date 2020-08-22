using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ListORama.DataAccess;
using ListORama.Models;

namespace ListORama.Controllers
{
    public class ListGroupsController : Controller
    {
        private readonly ApplicationDBContext _context;
        private int userID;
        
        public ListGroupsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: ListGroups1
        public async Task<IActionResult> Index()
        {            
            userID = Convert.ToInt16(@TempData.Peek("currentUserUserId"));
            var list1 = (from g in _context.listgroups
                         where g.userID == userID
                         select new ListGroup
                         {
                             listGroupID = g.listGroupID,
                             listGroupName = g.listGroupName,
                             userID = g.userID
                         }).ToList();
            return View(list1);
            //return View(await _context.listgroups.ToListAsync());
        }

        // GET: ListGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listGroup = await _context.listgroups
                .FirstOrDefaultAsync(m => m.listGroupID == id);
            if (listGroup == null)
            {
                return NotFound();
            }

            return View(listGroup);
        }

        // GET: ListGroups/Create
        public IActionResult Create()
        {
            
            userID = Convert.ToInt16(TempData.Peek("currentUserUserId"));
            ListGroup newListGroup = new ListGroup();
            newListGroup.listGroupName = "";
            newListGroup.listGroupID = 0;
            newListGroup.userID = userID;
            return View("Create", newListGroup);
        }

        // POST: ListGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("listGroupID,listGroupName,userID")] ListGroup listGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(listGroup);
        }

        // GET: ListGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listGroup = await _context.listgroups.FindAsync(id);
            if (listGroup == null)
            {
                return NotFound();
            }
            return View(listGroup);
        }

        // POST: ListGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("listGroupID,listGroupName,userID")] ListGroup listGroup)
        {
            if (id != listGroup.listGroupID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListGroupExists(listGroup.listGroupID))
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
            return View(listGroup);
        }

        // GET: ListGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listGroup = await _context.listgroups
                .FirstOrDefaultAsync(m => m.listGroupID == id);
            if (listGroup == null)
            {
                return NotFound();
            }

            return View(listGroup);
        }

        // POST: ListGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listGroup = await _context.listgroups.FindAsync(id);
            _context.listgroups.Remove(listGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListGroupExists(int id)
        {
            return _context.listgroups.Any(e => e.listGroupID == id);
        }
    }
}
