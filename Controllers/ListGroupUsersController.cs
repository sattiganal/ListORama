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
    public class ListGroupUsersController : Controller
    {
        private readonly ApplicationDBContext _context;
        public int selectedGroupID;
        public int userID;
        public ListGroupUsersController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: ListGroupUsers
        public async Task<IActionResult> Index()
        {
            userID = Convert.ToInt16(@TempData["UserID"]);
            userID = 1;
            TempData["UserID"] = userID;
            var dropdownVD1 = new SelectList(_context.listgroups.Where(x => x.userID == userID).OrderBy(x => x.listGroupName).ToList(), "listGroupID", "listGroupName");
            ViewData["GroupDataVD"] = dropdownVD1;
            selectedGroupID = Convert.ToInt32(dropdownVD1.Select(x => x.Value).First());
            loadGroups(selectedGroupID);
            return View(await _context.listgroupusers.ToListAsync());
        }

        private void loadGroups(int groupID)
        {
            var list1 = (from lg in _context.listgroupusers
                         join u in _context.users on lg.userID equals u.userID join g in _context.listgroups on lg.listGroupID equals g.listGroupID
                         where lg.listGroupID == groupID
                         select new ListGroupUser
                         {
                             listGroupUserID = lg.listGroupUserID,
                             user = u,
                             listGroup = g
                         }).OrderBy(x => x.user.lastName).ThenBy(x => x.user.firstName).ToList();
                     
            ViewBag.GroupData = list1;
            //return list1;
        }

        [HttpGet]
        public ActionResult GroupChanged(int? val)
        {
            if (val != null)
            {
                loadGroups(val.Value);
            }
            return PartialView("UserGridPV", ViewBag.GroupData);
            
        }

        // GET: ListGroupUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listGroupUser = await _context.listgroupusers
                .FirstOrDefaultAsync(m => m.listGroupUserID == id);
            if (listGroupUser == null)
            {
                return NotFound();
            }

            return View(listGroupUser);
        }

        // GET: ListGroupUsers/Create
        public IActionResult Create()
        {
            //using viewdata 
            userID = Convert.ToInt16(@TempData["UserID"]);
            TempData["UserID"] = userID;
            var dropdownVD = new SelectList(_context.users.ToList(), "userID", "fullName");
            ViewData["StudDataVD"] = dropdownVD;
            var dropdownVD1 = new SelectList(_context.listgroups.Where(x => x.userID == userID).ToList(), "listGroupID", "listGroupName");
            ViewData["GroupDataVD"] = dropdownVD1;
            return View();
        }

        // POST: ListGroupUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("listGroupUserID,listGroupID,userID")] ListGroupUser listGroupUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listGroupUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(listGroupUser);
        }

        // GET: ListGroupUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listGroupUser = await _context.listgroupusers.FindAsync(id);
            if (listGroupUser == null)
            {
                return NotFound();
            }
            return View(listGroupUser);
        }

        // POST: ListGroupUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("listGroupUserID")] ListGroupUser listGroupUser)
        {
            if (id != listGroupUser.listGroupUserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listGroupUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListGroupUserExists(listGroupUser.listGroupUserID))
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
            return View(listGroupUser);
        }

        // GET: ListGroupUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listgroupuser =
                (from gu in _context.listgroupusers
                 join u in _context.users on gu.userID equals u.userID
                 where gu.listGroupUserID == id
                 select new ListGroupUser
                 {
                     listGroupUserID = gu.listGroupUserID,
                     user = u
                 }).FirstOrDefault();
                
                //_context.listgroupusers
                //.FirstOrDefaultAsync(m => m.listGroupUserID == id);
            if (listgroupuser == null)
            {
                return NotFound();
            }
            return View(listgroupuser);
        }

        // POST: ListGroupUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listGroupUser = await _context.listgroupusers.FindAsync(id);
            _context.listgroupusers.Remove(listGroupUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListGroupUserExists(int id)
        {
            return _context.listgroupusers.Any(e => e.listGroupUserID == id);
        }
    }
}
