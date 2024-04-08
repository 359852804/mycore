using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.AspNetCore.Identity;
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

        public async Task<bool> AddItemAsync(TodoItem newItem, IdentityUser user)
        {
            newItem.Id = Guid.NewGuid();
            newItem.isDone = false;
            newItem.DueAt = DateTimeOffset.Now.AddDays(3);
            newItem.UserId = user.Id;
            _context.Items.Add(newItem);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<TodoItem[]> GetIncompleteItemsAsync(IdentityUser user)
        {
            var items = await _context.Items.Where(x => x.isDone == false && x.UserId == user.Id).ToArrayAsync();
            return items;
        }

        public async Task<bool> MarkDoneAsync(Guid id, IdentityUser user)
        {

            var item = await _context.Items.Where(x => x.Id == id && x.UserId == user.Id).SingleOrDefaultAsync();

            if (item == null) return false;

            item.isDone = true;
            var saveResult = await _context.SaveChangesAsync();

            return saveResult == 1;
            throw new NotImplementedException();
        }
    }
}
