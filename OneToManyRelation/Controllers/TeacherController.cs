using Application.Common.CQRS.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace StudentTeacherManagement.Controllers
{
    /// <summary>
    /// Manages teacher-related operations such as creating and listing teachers.
    /// </summary>
    public class TeacherController : Controller
    {
        #region Private Fields

        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for TeacherController.
        /// </summary>
        /// <param name="mediator">Mediator for sending commands and queries.</param>
        /// <param name="logger">Serilog logger for logging operations.</param>
        public TeacherController(IMediator mediator, Serilog.ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Displays the teacher creation form.
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            _logger.Information("Accessed the teacher creation form.");
            return View();
        }

        /// <summary>
        /// Handles the teacher creation POST request.
        /// </summary>
        /// <param name="model">The teacher data to be added.</param>
        [HttpPost]
        public async Task<IActionResult> Create(AddTeacherCommand model)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warning("Invalid model state for AddTeacherCommand.");
                return View(model);
            }

            var command = new AddTeacherCommand
            {
                Name = model.Name,
                ImageFile = model.ImageFile,
                Age = model.Age,
                Sex = model.Sex
            };

            _logger.Information("Creating a new teacher with name {Name}.", model.Name);
            await _mediator.Send(command);
            _logger.Information("Teacher created successfully.");

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Displays the list of teachers.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.Information("Accessed the teacher list.");
            return View();
        }

        #endregion
    }
}
