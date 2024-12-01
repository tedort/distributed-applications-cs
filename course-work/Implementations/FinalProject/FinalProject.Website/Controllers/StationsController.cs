using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data.Entities;
using FinalProject.Website.Data;
using Microsoft.AspNetCore.Authorization;
using X.PagedList.Extensions;

namespace FinalProject.Website.Controllers
{
    public class StationsController : Controller
    {
        private readonly FinalProjectWebsiteContext _context;

        public StationsController(FinalProjectWebsiteContext context)
        {
            _context = context;
        }

        // GET: Stations
        public async Task<IActionResult> Index(string sortOrder, int? page)
        {
            int pageCurrent = page ?? 1;
            int pageMaxSize = 2;

            var stations = _context.Station
                .Include(s => s.Name)
                .Include(s => s.TracksCount)
                .Include(s => s.Address)
                .Include(s => s.PlatfromsCount)
                .AsQueryable();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name-desc" : "name-asc";
            ViewBag.TrackSortParam = sortOrder == "track-asc" ? "track-desc" : "track-asc";
            ViewBag.AddressSortParam = sortOrder == "address-asc" ? "address-desc" : "address-asc";
            ViewBag.PlatformSortParam = sortOrder == "platform-asc" ? "platform-desc" : "platform-asc";

            switch (sortOrder)
            {
                case "name-desc":
                    stations = stations.OrderByDescending(c => c.Name);
                    break;
                case "name-asc":
                    stations = stations.OrderBy(c => c.Name);
                    break;
                case "track-desc":
                    stations = stations.OrderByDescending(c => c.TracksCount);
                    break;
                case "track-asc":
                    stations = stations.OrderBy(c => c.TracksCount);
                    break;
                case "address-desc":
                    stations = stations.OrderByDescending(c => c.Address);
                    break;
                case "address-asc":
                    stations = stations.OrderBy(c => c.Address);
                    break;
                case "platform-desc":
                    stations = stations.OrderByDescending(c => c.PlatfromsCount);
                    break;
                case "platform-asc":
                    stations = stations.OrderBy(c => c.PlatfromsCount);
                    break;
                default:
                    break;
            }

            return View(_context.Station.ToPagedList(pageCurrent, pageMaxSize));
        }

        // GET: Stations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var station = await _context.Station
                .FirstOrDefaultAsync(m => m.Id == id);
            if (station == null)
            {
                return NotFound();
            }

            return View(station);
        }

        // GET: Stations/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,TracksCount,Address,PlatfromsCount,HasCashDesk,Id")] Station station)
        {
            if (ModelState.IsValid)
            {
                _context.Add(station);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(station);
        }

        // GET: Stations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var station = await _context.Station.FindAsync(id);
            if (station == null)
            {
                return NotFound();
            }
            return View(station);
        }

        // POST: Stations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,TracksCount,Address,PlatfromsCount,HasCashDesk,Id")] Station station)
        {
            if (id != station.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(station);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StationExists(station.Id))
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
            return View(station);
        }

        // GET: Stations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var station = await _context.Station
                .FirstOrDefaultAsync(m => m.Id == id);
            if (station == null)
            {
                return NotFound();
            }

            return View(station);
        }

        // POST: Stations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var station = await _context.Station.FindAsync(id);
            if (station != null)
            {
                _context.Station.Remove(station);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StationExists(int id)
        {
            return _context.Station.Any(e => e.Id == id);
        }
    }
}
