using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace api_ecommerce.Services
{
    public class LogService
    {
        private readonly ApplicationDbContext _context;
        public LogService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Log>> GetLogsAsync(int pagina = 0, int quantidade = 10)
        {
            return await _context.Logs
                .OrderByDescending(log => log.Timestamp)
                .Skip(pagina).Take(quantidade)
                .AsQueryable()
                .ToListAsync();
        }

    }
}
