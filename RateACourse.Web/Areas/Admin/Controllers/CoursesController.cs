﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RateACourse.Core.Entities;
using RateACourse.Web.ViewModels;
using System.Net.Http.Headers;
using RateACourse.Core.Data;
using RateACourse.Web.Areas.Account.ViewModels;
using RateACourse.Web.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using RateACourse.Core.Services.Interfaces;

namespace RateACourse.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ICourseService _courseService;

        public CoursesController(ApplicationDbContext courseRateDbContext, ICourseService courseService)
        {
            applicationDbContext = courseRateDbContext;
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _courseService.GetAllAsync();
            if (result.IsSuccess)
            {
                var coursesIndexViewModel = new CoursesIndexViewModel
                {
                    Errors = new List<string>(),
                    Courses = result.Items
                    .Select(c => new BaseViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                    }),
                };
                return View(coursesIndexViewModel);
            }
            return View(new CoursesIndexViewModel { Errors = result.Errors });
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var result = await _courseService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound();
            }
            var coursesDetailViewModel = new CoursesDetailViewModel
            {
                Id = result.Items.First().Id,
                Name = result.Items.First().Name,
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
                    Name = coursesCreateViewModel.Name,
                };
                applicationDbContext.Add(course);
                await applicationDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coursesCreateViewModel);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            var course = await applicationDbContext.Courses.FindAsync(id);
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
                var course = await applicationDbContext
                        .Courses
                        .FirstOrDefaultAsync(c => c.Id == coursesEditViewModel.Id);
                if (course == null)
                {
                    return NotFound();
                }
                course.Name = coursesEditViewModel.Name;
                try
                {
                    applicationDbContext.Update(course);
                    await applicationDbContext.SaveChangesAsync();
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
        public async Task<IActionResult> ConfirmDelete(long id)
        {
            var course = await applicationDbContext.Courses
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
            var course = await applicationDbContext.Courses.FindAsync(id);
            if (course != null)
            {
                applicationDbContext.Courses.Remove(course);
            }
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(long id)
        {
            return applicationDbContext.Courses.Any(e => e.Id == id);
        }
    }
}