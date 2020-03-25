using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Introduction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Introduction.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly AspNetCoreIntroductionContext _context;

        public EmployeesController(AspNetCoreIntroductionContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Employees = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (Employees == null)
            {
                return NotFound();
            }

            return View(Employees);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Founded, Fax")] Employees Employees)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Employees);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Employees);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Employees = await _context.Employees.FindAsync(id);
            if (Employees == null)
            {
                return NotFound();
            }
            return View(Employees);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Founded, Fax")] Employees Employees)
        {
            if (id != Employees.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Employees);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeesExists(Employees.EmployeeId))
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
            return View(Employees);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Employees = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (Employees == null)
            {
                return NotFound();
            }

            return View(Employees);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Employees = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(Employees);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
