using Microsoft.AspNetCore.Mvc;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.Models;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        private readonly UserManager<IdentityUser> _userManager;

        public TodoController(ITodoItemService todoItemService, UserManager<IdentityUser> userManager)
        {
            _todoItemService = todoItemService;
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var items = await _todoItemService.GetIncompleteItemsAsync(currentUser);

            var model = new TodoViewModel()
            {
                items = items
            };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            Console.WriteLine("userid=" + user.Id);
            if(user == null) return Challenge();

            var successful = await _todoItemService.AddItemAsync(newItem,user);

            if (!successful)
            {
                return BadRequest("添加失败");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MarkDone(Guid id)
        {
            if(id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var successful = await _todoItemService.MarkDoneAsync(id, user);
            if (!successful)
            {
                return BadRequest("fail");
            }

            return RedirectToAction("Index");
        }
    }
}
