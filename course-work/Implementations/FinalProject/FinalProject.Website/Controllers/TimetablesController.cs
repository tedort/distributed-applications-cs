using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data.Entities;
using FinalProject.Website.Data;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Website.Controllers
{
    public class TimetablesController : Controller
    {
        private readonly FinalProjectWebsiteContext _context;

        public TimetablesController(FinalProjectWebsiteContext context)
        {
            _context = context;
        }

        // GET: Timetables
        public async Task<IActionResult> Index(int? searchTrainId, int? searchStationId, string sortOrder, int? page)
        {
            int pageCurrent = page ?? 1;
            int pageMaxSize = 6;

            var timetables = _context.Timetable
                .Include(t => t.Station)
                .Include(t => t.Train)
                .AsQueryable();

            ViewBag.Trains = new SelectList(_context.Train, "Id", "TrainNumber");
            ViewBag.Stations = new SelectList(_context.Station, "Id", "Name");

            ViewBag.TrainSearch = searchTrainId.ToString();
            if (searchTrainId.HasValue)
            {
                timetables = timetables.Where(t => t.Train.Id == searchTrainId);
            }

            ViewBag.StationSearch = searchStationId.ToString();
            if (searchStationId.HasValue)
            {
                timetables = timetables.Where(t => t.StationId == searchStationId);
            }


            ViewBag.CurrentSort = sortOrder;
            ViewBag.TrainSortParam = string.IsNullOrEmpty(sortOrder) ? "train-desc" : "train-asc";
            ViewBag.StationSortParam = sortOrder == "station-asc" ? "station-desc" : "station-asc";
            ViewBag.ArrivalSortParam = sortOrder == "arrival-asc" ? "arrival-desc" : "arrival-asc";
            ViewBag.DepartingSortParam = sortOrder == "departing-asc" ? "departing-desc" : "departing-asc";
            ViewBag.TrackSortParam = sortOrder == "track-asc" ? "track-desc" : "track-asc";

            

            switch (sortOrder)
            {
                case "train-desc":
                    timetables = timetables.OrderByDescending(t => t.Train.TrainNumber);
                    break;
                case "train-asc":
                    timetables = timetables.OrderBy(t => t.Train.TrainNumber);
                    break;
                case "station-desc":
                    timetables = timetables.OrderByDescending(t => t.Station.Name);
                    break;
                case "station-asc":
                    timetables = timetables.OrderBy(t => t.Station.Name);
                    break;
                case "arrival-asc":
                    timetables = timetables.OrderBy(t => t.ArrivalTime);
                    break;
                case "arrival-desc":
                    timetables = timetables.OrderByDescending(t => t.ArrivalTime);
                    break;
                case "departing-asc":
                    timetables = timetables.OrderBy(t => t.DepartureTime);
                    break;
                case "departing-desc":
                    timetables = timetables.OrderByDescending(t => t.DepartureTime);
                    break;
                case "track-asc":
                    timetables = timetables.OrderBy(t => t.PlatformNumber);
                    break;
                case "track-desc":
                    timetables = timetables.OrderByDescending(t => t.PlatformNumber);
                    break;
                default:
                    timetables = timetables.OrderBy(t => t.Train.TrainNumber);
                    break;
            }

            return View(timetables.ToPagedList(pageCurrent, pageMaxSize));
        }


        // GET: Timetables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetable = await _context.Timetable
                .Include(t => t.Station)
                .Include(t => t.Train)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timetable == null)
            {
                return NotFound();
            }

            return View(timetable);
        }

        // GET: Timetables/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["StationId"] = new SelectList(_context.Station, "Id", "Name");
            ViewData["TrainId"] = new SelectList(_context.Train, "Id", "TrainNumber");
            return View();
        }

        // POST: Timetables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainId,StationId,ArrivalTime,DepartureTime,PlatformNumber,IsStartStation,IsFinalStation,Id")] Timetable timetable)
        {
            if (ModelState.IsValid)
            {
                var station = await _context.Station.FirstOrDefaultAsync(s => s.Id == timetable.StationId);

                if (station == null)
                {
                    ModelState.AddModelError("StationId", "The selected station does not exist.");
                }

                if (timetable.PlatformNumber > station.TracksCount)
                {
                    ModelState.AddModelError("PlatformNumber",
                        $"Въведеният коловоз превишава броя на коловозите на гара {station.Name}: {station.TracksCount}");
                }

                if (timetable.ArrivalTime > timetable.DepartureTime)
                {
                    ModelState.AddModelError("ArrivalTime", "Времето на пристигане не може да е след времето на заминаване!");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Add(timetable);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving timetable: {ex.Message}");
                        ModelState.AddModelError("", "An error occurred while saving the timetable.");
                    }
                }
            }

            ViewData["StationId"] = new SelectList(_context.Station, "Id", "Name", timetable.StationId);
            ViewData["TrainId"] = new SelectList(_context.Train, "Id", "TrainNumber", timetable.TrainId);
            return View(timetable);
        }

        // GET: Timetables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetable = await _context.Timetable.FindAsync(id);
            if (timetable == null)
            {
                return NotFound();
            }
            ViewData["StationId"] = new SelectList(_context.Station, "Id", "Name", timetable.StationId);
            ViewData["TrainId"] = new SelectList(_context.Train, "Id", "TrainNumber", timetable.TrainId);
            return View(timetable);
        }

        // POST: Timetables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainId,StationId,ArrivalTime,DepartureTime,PlatformNumber,IsStartStation,IsFinalStation,Id")] Timetable timetable)
        {
            if (id != timetable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(timetable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimetableExists(timetable.Id))
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
            ViewData["StationId"] = new SelectList(_context.Station, "Id", "Name", timetable.StationId);
            ViewData["TrainId"] = new SelectList(_context.Train, "Id", "TrainNumber", timetable.TrainId);
            return View(timetable);
        }

        // GET: Timetables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var timetable = await _context.Timetable
                .Include(t => t.Station)
                .Include(t => t.Train)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timetable == null)
            {
                return NotFound();
            }

            return View(timetable);
        }

        // POST: Timetables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var timetable = await _context.Timetable.FindAsync(id);
            if (timetable != null)
            {
                _context.Timetable.Remove(timetable);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> LiveTable(int? stationId, int? page)
        {
            int pageCurrent = page ?? 1;
            int pageMaxSize = 6;

            var stations = _context.Station
                .Select(s => new { s.Id, s.Name })
                .ToList();

            ViewBag.Stations = stations;

            if (stationId == null)
            {
                return View(Enumerable.Empty<Timetable>().ToPagedList(pageCurrent, pageMaxSize));
            }

            var timetableData = _context.Timetable
                .Include(t => t.Train)
                .Include(t => t.Station)
                .Where(t => t.StationId == stationId)
                .Select(t => new
                {
                    t.Train.TrainNumber,
                    ArrivesFrom = _context.Timetable
                        .Where(tt => tt.TrainId == t.TrainId && tt.IsStartStation)
                        .OrderByDescending(tt => tt.DepartureTime)
                        .Select(tt => tt.Station.Name)
                        .FirstOrDefault(),
                    t.ArrivalTime,
                    DepartsTo = _context.Timetable
                        .Where(tt => tt.TrainId == t.TrainId && tt.IsFinalStation)
                        .OrderBy(tt => tt.ArrivalTime)
                        .Select(tt => tt.Station.Name)
                        .FirstOrDefault(),
                    t.DepartureTime,
                    t.PlatformNumber
                })
                .ToList();

            return View(timetableData.ToPagedList(pageCurrent, pageMaxSize));
        }

        private bool TimetableExists(int id)
        {
            return _context.Timetable.Any(e => e.Id == id);
        }
    }
}
