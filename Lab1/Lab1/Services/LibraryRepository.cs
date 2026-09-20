using Lab1.Interfaces;
using Lab1.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Services
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly AppDbContext _context;

        public LibraryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LibraryItem item)
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.LibraryItems.FindAsync(id);


            if (item != null)
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
            }
      

        }

        public async Task<List<LibraryItem>> GetAllAsync()
        {
            return await _context.LibraryItems.AsNoTracking().ToListAsync();
        }

        public async Task<LibraryItem?> GetByIdAsync(int id)
        {
            return await _context.LibraryItems.FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<List<T>> GetByTypeAsync<T>() where T : LibraryItem
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task UpdateAsync(LibraryItem item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveColumnAsync(int columnId)
        {
            var column = await _context.Set<NewspaperColumn>().FindAsync(columnId);
            if (column != null)
            {
                _context.Set<NewspaperColumn>().Remove(column);
                await _context.SaveChangesAsync();
            }
        }

        
    }
}
