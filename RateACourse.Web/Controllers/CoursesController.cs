using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RateACourse.Core.Entities;
using RateACourse.Core.Data;
using RateACourse.Web.ViewModels;
using System.Net.Http.Headers;

namespace RateACourse.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CoursesController(ApplicationDbContext courseRateDbContext)
        {
            _applicationDbContext = courseRateDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var coursesIndexViewModel = new CoursesIndexViewModel
            {
                Courses = await _applicationDbContext.Courses
                .Select(c => new BaseViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync()
            };
            return View(coursesIndexViewModel);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            var course = await _applicationDbContext
                .Courses
                .FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            var coursesDetailViewModel = new CoursesDetailViewModel
            {
                Id = course.Id,
                Name = course.Name,
            };
            return View(coursesDetailViewModel);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CoursesCreateViewModel coursesCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                var course = new Course 
                {
                    Name  = coursesCreateViewModel.Name,
                };
                _applicationDbContext.Add(course);
                await _applicationDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coursesCreateViewModel);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            var course = await _applicationDbContext.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var coursesEditViewModel = new CoursesEditViewModel
            {
                Id = course.Id,
                Name = course.Name,
            };
            return View(coursesEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CoursesEditViewModel coursesEditViewModel)
        {
            if (ModelState.IsValid)
            {
                var course = await _applicationDbContext
                        .Courses
                        .FirstOrDefaultAsync(c => c.Id == coursesEditViewModel.Id);
                if (course == null)
                {
                    return NotFound();
                }
                course.Name = coursesEditViewModel.Name;
                try
                {
                    _applicationDbContext.Update(course);
                    await _applicationDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException dbUpdateException)
                {
                    Console.WriteLine(dbUpdateException.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(coursesEditViewModel);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> ConfirmDelete(long? id)
        {
            var course = await _applicationDbContext.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            var coursesDeleteViewModel = new CoursesDeleteViewModel
            {
                Id = course.Id,
                Name = course.Name,
            };
            return View(coursesDeleteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var course = await _applicationDbContext.Courses.FindAsync(id);
            if (course != null)
            {
                _applicationDbContext.Courses.Remove(course);
            }
            await _applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(long id)
        {
          return _applicationDbContext.Courses.Any(e => e.Id == id);
        }
    }
}
