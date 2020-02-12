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
    public class LogManager : ILogManager
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LogManager(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LogDTO>> GetAllAsync()
        {
            var logs = await _context.Logs
                .Include(l => l.DoneBy)
                .ToListAsync();
            return _mapper.Map<IEnumerable<LogDTO>>(logs);
        }

        public async Task<LogDTO> GetByIdAsync(int? id)
        {
            var log = await _context.Logs
                .Include(l => l.DoneBy)
                .FirstOrDefaultAsync(m => m.Id == id);

            return _mapper.Map<LogDTO>(log);
        }

        public async Task<LogDTO> CreateAsync(LogDTO logDTO)
        {
            var log = _mapper.Map<Log>(logDTO);
            _context.Add(log);
            await _context.SaveChangesAsync();

            return _mapper.Map<LogDTO>(log);
        }

        public async Task<LogDTO> UpdateAsync(LogDTO logDTO)
        {
            var log = _mapper.Map<Log>(logDTO);
            _context.Update(log);
            await _context.SaveChangesAsync();

            return _mapper.Map<LogDTO>(log);
        }

        public bool ExistAsync(int id)
        {
            return _context.Logs.Any(e => e.Id == id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var log = await _context.Logs.FindAsync(id);
                _context.Logs.Remove(log);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (ArgumentNullException e)
            {
                throw new System.ArgumentNullException("Pask", e);
            }
        }
    }
}
