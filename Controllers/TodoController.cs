using Microsoft.AspNetCore.Mvc;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.Models;
using System.Threading.Tasks;

namespace AspNetCoreTodo.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;


        public TodoController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var items = await _todoItemService.GetIncompleteItemsAsync();

            var model = new TodoViewModel()
            {
                items = items
            };

            return View(model);
        }
    }
}
