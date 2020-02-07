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

        public LogManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Log>> GetAllAsync()
        {
            return await _context.Logs
                .Include(l => l.DoneBy)
                .ToListAsync();
        }

        public async Task<Log> GetByIdAsync(int? id)
        {
            return await _context.Logs
                .Include(l => l.DoneBy)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Log> CreateAsync(Log log)
        {          
            _context.Add(log);
            await _context.SaveChangesAsync();
            return log;
        }

        public async Task<Log> UpdateAsync(Log log)
        {
            _context.Update(log);
            await _context.SaveChangesAsync();
            return log;
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
            catch (Exception)
            {
                throw;
            }
            
        }



        //public async Task<Log> DeleteAsync(int id)
        //{
        //    return await _context.Logs.AnyAsync(e => e.Id == id);
        //}





        //Task<bool> IRepository<Log>.DeleteAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<Log> DeleteAsync(Log log)
        //{
        //    //var log = await _context.Logs.FindAsync(id);
        //    _context.Logs.Remove(log);
        //    await _context.SaveChangesAsync();            
        //}

        //public async Task<Log> DeleteAsync(int id)
        //{
        //    var log = await _context.Logs.FindAsync(id);
        //    _context.Logs.Remove(log);
        //    await _context.SaveChangesAsync();
        //}
    }
}
