using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using WebMvc.Models;
using WebMvc.Service;

namespace WebMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TaskService _taskService;

        public HomeController(ILogger<HomeController> logger, TaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        public IActionResult Index()
        {
            return View(_taskService.GetAllTasks().Select(t => TaskViewModel.FromTask(t)));
        }

        // GET: /HelloWorld/Edit/{id}
        public IActionResult Edit([FromRoute] int id)
        {
            var theTask = _taskService.FindTaskByID(id);
            var taskEditModel = TaskEditModel.FromTask(theTask);
            return View(taskEditModel);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Title,Content,DueDate")] TaskEditModel task)
        {
            if (ModelState.IsValid)
            {
                _taskService.UpdateTaskByID(id, task.Title, task.Content, task.DueDate);
                return RedirectToAction("ViewTask", new { id = id });
            }
            else
            {
                return View(task);
            }
        }

        public IActionResult ViewTask([FromRoute] int id)
        {
            var theTask = _taskService.FindTaskByID(id);
            return View(TaskViewModel.FromTask(theTask));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Content,DueDate")] TaskEditModel task)
        {
            if (ModelState.IsValid)
            {
                _taskService.AddNewTask(task.Title, task.Content, task.DueDate);
                return RedirectToAction("Index");
            }
            return View(task);
        }
    }
}
