using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CleanAspNetCoreWithFreeMetronic.Data;
using CleanAspNetCoreWithFreeMetronic.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using CleanAspNetCoreWithFreeMetronic.Services;

namespace CleanAspNetCoreWithFreeMetronic.Controllers
{
    public class JobTasksController : Controller
    {
        private readonly IJobTaskManager _jobTaskManager;
        private readonly ILogger<JobTasksController> _logger;
        private readonly ApplicationDbContext _context;

        public JobTasksController(ILogger<JobTasksController> logger, IJobTaskManager jobTaskManager, ApplicationDbContext context)
        {
            _logger = logger;
            _jobTaskManager = jobTaskManager;
            _context = context;
        }

        // GET: JobTasks
        public async Task<IActionResult> Index()
        {
            var jobTasks = await _jobTaskManager.GetAllAsync();           

            return View(jobTasks);
        }

        // GET: JobTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var jobTask = await _jobTaskManager.GetByIdAsync(id);

            if (jobTask == null)
            {
                return NotFound();
            }

            return View(jobTask);
        }

        // GET: JobTasks/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name");
            ViewData["ResponsibleId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: JobTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TaskName,TaskStart,EstimateHours,TaskDescription,Completed,ProjectId,ResponsibleId")] JobTaskDTO jobTaskDTO)
        {
            if (ModelState.IsValid)
            {
                await _jobTaskManager.CreateAsync(jobTaskDTO);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", jobTaskDTO.ProjectId);
            ViewData["ResponsibleId"] = new SelectList(_context.Users, "Id", "UserName", jobTaskDTO.ResponsibleId);

            return View(jobTaskDTO);
        }

        // GET: JobTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobTask = await _jobTaskManager.GetByIdAsync(id);          

            if (jobTask == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", jobTask.ProjectId);
            ViewData["ResponsibleId"] = new SelectList(_context.Users, "Id", "UserName", jobTask.ResponsibleId);
            return View(jobTask);
        }

        // POST: JobTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TaskName,TaskStart,EstimateHours,TaskDescription,Completed,ProjectId,ResponsibleId")] JobTaskDTO jobTaskDTO)
        {
            if (id != jobTaskDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _jobTaskManager.UpdateAsync(jobTaskDTO);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(_jobTaskManager.ExistAsync(jobTaskDTO.Id)))
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
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Name", jobTaskDTO.ProjectId);
            ViewData["ResponsibleId"] = new SelectList(_context.Users, "Id", "UserName", jobTaskDTO.ResponsibleId);

            return View(jobTaskDTO);
        }

        // GET: JobTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobTask = await _jobTaskManager.GetByIdAsync(id);

            if (jobTask == null)
            {
                return NotFound();
            }

            return View(jobTask);
        }

        // POST: JobTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _jobTaskManager.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
