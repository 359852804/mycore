using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreTodo.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;
        public TodoItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddItemAsync(TodoItem newItem)
        {
            newItem.Id = Guid.NewGuid();
            newItem.isDone = false;
            newItem.DueAt = DateTimeOffset.Now.AddDays(3);
            _context.Items.Add(newItem);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<TodoItem[]> GetIncompleteItemsAsync()
        {
            var items = await _context.Items.Where(x => x.isDone == false).ToArrayAsync();
            return items;
        }

        public async Task<bool> MarkDoneAsync(Guid id)
        {

            var item = await _context.Items.Where(x => x.Id == id).SingleOrDefaultAsync();

            if (item == null) return false;

            item.isDone = true;
            var saveResult = await _context.SaveChangesAsync();

            return saveResult == 1;
            throw new NotImplementedException();
        }
    }
}
