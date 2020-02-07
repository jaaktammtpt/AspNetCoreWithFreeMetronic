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
        private readonly IMapper _mapper;
        private readonly ILogger<Log> _logger;
        private readonly ApplicationDbContext _context;

        public LogsController(ILogger<Log> logger,ILogManager logManager, IMapper mapper, ApplicationDbContext context)
        {
            _logger = logger;
            _logManager = logManager;
            _mapper = mapper;
            _context = context;
        }

        // GET: Logs
        public async Task<IActionResult> Index()
        {
            var logs = await _logManager.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<LogDTO>>(logs);

            return View(dto);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _logManager.GetByIdAsync(id);
            var dto = _mapper.Map<LogDTO>(log);

            if (log == null)
            {
                return NotFound();
            }

            return View(dto);
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
                var log = _mapper.Map<Log>(logDTO);
                await _logManager.CreateAsync(log);

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

            var log = await _logManager.GetByIdAsync(id);
            //var logDTO = await _context.LogDTO.FindAsync(id);
            var dto = _mapper.Map<LogDTO>(log);

            if (log == null)
            {
                return NotFound();
            }

            ViewData["JobTaskId"] = new SelectList(_context.JobTasks, "Id", "TaskName");
            ViewData["DoneById"] = new SelectList(_context.Users, "Id", "UserName");

            return View(dto);
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

            var log = _mapper.Map<Log>(logDTO);

            if (ModelState.IsValid)
            {
                try
                {
                    //var log = _mapper.Map<Log>(logDTO);
                    //_context.Update(logDTO);
                    await _logManager.UpdateAsync(log);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(_logManager.ExistAsync(log.Id)))
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

            var dto = _mapper.Map<LogDTO>(log);

            return View(dto);
        }



        // GET: Logs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var log = await _logManager.GetByIdAsync(id);
            var dto = _mapper.Map<LogDTO>(log);

            if (dto == null)
            {
                return NotFound();
            }

            return View(dto);
        }

        // POST: Logs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var log = await _logManager.GetByIdAsync(id);
            await _logManager.DeleteAsync(id);
            //_context.LogDTO.Remove(logDTO);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
