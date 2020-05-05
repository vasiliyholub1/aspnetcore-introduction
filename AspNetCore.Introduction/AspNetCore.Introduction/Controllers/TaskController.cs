using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Introduction.Controllers
{
    public class TaskController: Controller
    {
        // GET: Tasks
        public IActionResult Index()
        {
            return View();
        }

        // GET: Specific task info.
        public IActionResult Task(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var viewName = $"Task{id.Value}";

            return View(viewName);
        }
    }
}
