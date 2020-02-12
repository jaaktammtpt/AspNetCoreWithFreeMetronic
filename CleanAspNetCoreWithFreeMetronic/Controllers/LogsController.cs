using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CleanAspNetCoreWithFreeMetronic.Data;
using CleanAspNetCoreWithFreeMetronic.Models;
using AutoMapper;
using CleanAspNetCoreWithFreeMetronic.Services;
using Microsoft.Extensions.Logging;

namespace CleanAspNetCoreWithFreeMetronic.Controllers
{
    public class LogsController : Controller
    {
        private readonly ILogManager _logManager;
        private readonly ILogger<Log> _logger;
        private readonly ApplicationDbContext _context;

        public LogsController(ILogger<Log> logger,ILogManager logManager, ApplicationDbContext context)
        {
            _logger = logger;
            _logManager = logManager;
            _context = context;
        }

        // GET: Logs
        public async Task<IActionResult> Index()
        {
            var logs = await _logManager.GetAllAsync();

            return View(logs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var log = await _logManager.GetByIdAsync(id);

            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        // GET: Logs/Create
        public IActionResult Create()
        {
            ViewData["JobTaskId"] = new SelectList(_context.JobTasks, "Id", "TaskName");
            ViewData["DoneById"] = new SelectList(_context.Users, "Id", "UserName");            

            return View();
        }



        // POST: Logs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LogDate,TimeSpent,LogDescription,JobTaskId,DoneById")] LogDTO logDTO)
        {
            if (ModelState.IsValid)
            {
                await _logManager.CreateAsync(logDTO);

                return RedirectToAction(nameof(Index));
            }

            ViewData["JobTaskId"] = new SelectList(_context.JobTasks, "Id", "TaskName", logDTO.JobTaskId);
            ViewData["DoneById"] = new SelectList(_context.Users, "Id", "UserName", logDTO.DoneById);

            return View(logDTO);
        }

        // GET: Logs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logDTO = await _logManager.GetByIdAsync(id);

            if (logDTO == null)
            {
                return NotFound();
            }

            ViewData["JobTaskId"] = new SelectList(_context.JobTasks, "Id", "TaskName");
            ViewData["DoneById"] = new SelectList(_context.Users, "Id", "UserName");

            return View(logDTO);
        }

        // POST: Logs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LogDate,TimeSpent,LogDescription,JobTaskId,DoneById")] LogDTO logDTO)
        {
            if (id != logDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _logManager.UpdateAsync(logDTO);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(_logManager.ExistAsync(logDTO.Id)))
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

            ViewData["JobTaskId"] = new SelectList(_context.JobTasks, "Id", "TaskName", logDTO.JobTaskId);
            ViewData["DoneById"] = new SelectList(_context.Users, "Id", "UserName", logDTO.DoneById);

            return View(logDTO);
        }



        // GET: Logs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _logManager.GetByIdAsync(id);

            if (log == null)
            {
                return NotFound();
            }

            return View(log);
        }

        // POST: Logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _logManager.DeleteAsync(id);
            
            return RedirectToAction(nameof(Index));
        }
        
    }
}
