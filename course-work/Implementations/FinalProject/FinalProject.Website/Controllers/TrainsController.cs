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
    public class TrainsController : Controller
    {
        private readonly FinalProjectWebsiteContext _context;

        public TrainsController(FinalProjectWebsiteContext context)
        {
            _context = context;
        }

        // GET: Trains
        public async Task<IActionResult> Index(string searchTrainNumber, int? searchCategoryId, string sortOrder, int? page)
        {
            int pageCurrent = page ?? 1;
            int pageMaxSize = 5;

            var trains = _context.Train.Include(t => t.Category).AsQueryable();

            ViewBag.Categories = new SelectList(_context.Category, "Id", "CategoryName");

            ViewBag.TrainSearch = searchTrainNumber;
            if (!string.IsNullOrEmpty(searchTrainNumber))
            {
                trains = trains.Where(t => t.TrainNumber.Contains(searchTrainNumber));
            }

            ViewBag.CategorySearch = searchCategoryId.ToString();
            if (searchCategoryId.HasValue)
            {
                trains = trains.Where(t => t.CategoryId == searchCategoryId);
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TrainSortParam = string.IsNullOrEmpty(sortOrder) ? "train-desc" : "train-asc";
            ViewBag.CategorySortParam = sortOrder == "category-asc" ? "category-desc" : "category-asc";


            switch (sortOrder)
            {
                case "train-desc":
                    trains = trains.OrderByDescending(t => t.TrainNumber);
                    break;
                case "train-asc":
                    trains = trains.OrderBy(t => t.TrainNumber);
                    break;
                case "category-desc":
                    trains = trains.OrderByDescending(t => t.Category);
                    break;
                case "category-asc":
                    trains = trains.OrderByDescending(t => t.Category);
                    break;
                default:
                    break;
            }

            return View(trains.ToPagedList(pageCurrent, pageMaxSize));
        }

        // GET: Trains/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Train
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (train == null)
            {
                return NotFound();
            }

            return View(train);
        }

        // GET: Trains/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName");
            return View();
        }

        // POST: Trains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainNumber,CategoryId,Id")] Train train)
        {
            if (ModelState.IsValid)
            {
                _context.Add(train);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", train.CategoryId);
            return View(train);
        }

        // GET: Trains/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Train.FindAsync(id);
            if (train == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", train.CategoryId);
            return View(train);
        }

        // POST: Trains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainNumber,CategoryId,Id")] Train train)
        {
            if (id != train.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(train);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainExists(train.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "CategoryName", train.CategoryId);
            return View(train);
        }

        // GET: Trains/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var train = await _context.Train
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (train == null)
            {
                return NotFound();
            }

            return View(train);
        }

        // POST: Trains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var train = await _context.Train.FindAsync(id);
            if (train != null)
            {
                _context.Train.Remove(train);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainExists(int id)
        {
            return _context.Train.Any(e => e.Id == id);
        }
    }
}
