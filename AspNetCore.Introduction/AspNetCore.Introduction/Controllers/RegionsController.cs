using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Introduction.Interfaces;
using AspNetCore.Introduction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Introduction.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IDbRegionRepository _regionRepository;
        

        public RegionsController(IDbRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }

        // GET: Regions
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _regionRepository.GetAsync());
        }

        // GET: Regions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regions = await _regionRepository.FindAsync(id);
            if (regions == null)
            {
                return NotFound();
            }

            return View(regions);
        }

        // GET: Regions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Regions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegionId,RegionDescription")] Regions regions)
        {
            if (ModelState.IsValid)
            {
                _regionRepository.Insert(regions);
                await _regionRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(regions);
        }

        // GET: Regions/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regions = await _regionRepository.FindAsync(id);
            if (regions == null)
            {
                return NotFound();
            }
            return View(regions);
        }

        // POST: Regions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RegionId,RegionDescription")] Regions regions)
        {
            if (id != regions.RegionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _regionRepository.Update(regions);
                    await _regionRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegionsExists(regions.RegionId))
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
            return View(regions);
        }

        // GET: Regions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regions = await _regionRepository.FindAsync(id);
            if (regions == null)
            {
                return NotFound();
            }

            return View(regions);
        }

        // POST: Regions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var regions = await _regionRepository.FindAsync(id);
            _regionRepository.Delete(regions);
            await _regionRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegionsExists(int id)
        {
            return _regionRepository.Get().Any(e => e.RegionId == id);
        }
    }
}
