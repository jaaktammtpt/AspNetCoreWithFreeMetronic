using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CleanAspNetCoreWithFreeMetronic.Data;
using CleanAspNetCoreWithFreeMetronic.Models;
using CleanAspNetCoreWithFreeMetronic.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace CleanAspNetCoreWithFreeMetronic.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectManager _projectManager;
        private readonly ILogger<ProjectsController> _logger;
        //private readonly ApplicationDbContext _context;

        public ProjectsController(ILogger<ProjectsController> logger, IProjectManager projectManager)
        {
            _logger = logger;
            _projectManager = projectManager;
            //_context = context;
        }

        // GET: Project
        public async Task<IActionResult> Index()
        {
            var projects = await _projectManager.GetAllAsync();
            return View(projects);
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var project = await _projectManager.GetByIdAsync(id);
            //var dto = _mapper.Map<ProjectDTO>(project);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Begin,Finish,Budget,Rate")] ProjectDTO projectDTO)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(projectDTO);
                //await _context.SaveChangesAsync();
                //var project = _mapper.Map<Project>(projectDTO);
                await _projectManager.CreateAsync(projectDTO);
                return RedirectToAction(nameof(Index));
            }
            return View(projectDTO);
        }

        // GET: Project/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectManager.GetByIdAsync(id);
            //var dto = _mapper.Map<ProjectDTO>(project);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Begin,Finish,Budget,Rate")] ProjectDTO projectDTO)
        {
            if (id != projectDTO.Id)
            {
                return NotFound();
            }

            //var project = _mapper.Map<Project>(projectDTO);

            if (ModelState.IsValid)
            {
                try
                {
                    await _projectManager.UpdateAsync(projectDTO);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(_projectManager.ExistAsync(projectDTO.Id)))
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
            return View(projectDTO);
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _projectManager.GetByIdAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _projectManager.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
