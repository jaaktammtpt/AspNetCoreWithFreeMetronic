using AutoMapper;
using CleanAspNetCoreWithFreeMetronic.Models;
using CleanAspNetCoreWithFreeMetronic.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CleanAspNetCoreWithFreeMetronic.Data.Services
{
    public class JobTaskManager : IJobTaskManager
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public JobTaskManager(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<JobTaskDTO> CreateAsync(JobTaskDTO jobTaskDTO)
        {
            var jobTask = _mapper.Map<JobTask>(jobTaskDTO);
            _context.Add(jobTask);
            await _context.SaveChangesAsync();
            return _mapper.Map<JobTaskDTO>(jobTask);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var jobTask = await _context.JobTasks
                    .Include(j => j.Project)
                    .Include(j => j.Responsible)
                    .Include(j => j.TaskFiles)
                    .Include(j => j.Logs)
                    .FirstOrDefaultAsync(m => m.Id == id);

                _context.JobTasks.Remove(jobTask);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (ArgumentNullException e)
            {
                throw new System.ArgumentNullException("Pask", e);
            }
        }

        public bool ExistAsync(int id)
        {
            return _context.JobTasks.Any(e => e.Id == id);
        }

        public async Task<IEnumerable<JobTaskDTO>> GetAllAsync()
        {
            var jobtasks = await _context.JobTasks
                .Include(j => j.Project)
                .Include(j => j.Responsible)
                .Include(j => j.TaskFiles)
                .Include(j => j.Logs)
                .ToListAsync();

            return _mapper.Map<IEnumerable<JobTaskDTO>>(jobtasks);
        }

        public async Task<JobTaskDTO> GetByIdAsync(int? id)
        {
            var jobTask = await _context.JobTasks
                .Include(j => j.Project)
                .Include(j => j.Responsible)
                .Include(j => j.TaskFiles)
                .Include(j => j.Logs)
                .FirstOrDefaultAsync(m => m.Id == id);

            return _mapper.Map<JobTaskDTO>(jobTask);
        }

        public async Task<JobTaskDTO> UpdateAsync(JobTaskDTO jobTaskDTO)
        {
            var jobTask = _mapper.Map<JobTask>(jobTaskDTO);
            _context.Update(jobTask);
            await _context.SaveChangesAsync();
            return _mapper.Map<JobTaskDTO>(jobTask);
        }
    }
}
