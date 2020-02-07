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
    public class ProjectManager : IProjectManager
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProjectManager(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectDTO> CreateAsync(ProjectDTO projectDTO)
        {
            var project = _mapper.Map<Project>(projectDTO);
            _context.Add(project);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProjectDTO>(project);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var project = await _context.Projects
                    .Include(l => l.JobTasks)
                    .ThenInclude(n => n.Logs)
                    .Include(l => l.Teams)
                    .FirstOrDefaultAsync(m => m.Id == id);

                _context.Projects.Remove(project);
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
            return _context.Projects.Any(e => e.Id == id);
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllAsync()
        {
            var projects = await _context.Projects
                .Include(l => l.JobTasks)
                .Include(l => l.Teams)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ProjectDTO>>(projects);

        }

        public async Task<ProjectDTO> GetByIdAsync(int? id)
        {
            var project = await _context.Projects
                .Include(l => l.JobTasks)
                .ThenInclude(n => n.Logs)
                .Include(l => l.Teams)
                .FirstOrDefaultAsync(m => m.Id == id);

            return _mapper.Map<ProjectDTO>(project);
        }

        public async Task<ProjectDTO> UpdateAsync(ProjectDTO projectDTO)
        {
            var project = _mapper.Map<Project>(projectDTO);
            _context.Update(project);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProjectDTO>(project);
        }
    }
}
