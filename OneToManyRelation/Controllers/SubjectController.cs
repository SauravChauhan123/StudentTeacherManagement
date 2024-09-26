using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace StudentTeacherManagement.Controllers
{
    /// <summary>
    /// Controller responsible for handling operations related to subjects.
    /// </summary>
    public class SubjectController : Controller
    {
        #region Private Fields

        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectController"/> class.
        /// </summary>
        /// <param name="mediator">Mediator for handling CQRS commands and queries.</param>
        /// <param name="logger">Serilog logger for logging operations.</param>
        public SubjectController(IMediator mediator, Serilog.ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Displays the form to add a new subject.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            _logger.Information("Fetching languages and teachers for Add Subject form.");

            // Fetch available languages
            var createLanguage = new List<string> { "English", "Hindi", "German", "French", "Sanskrit", "Punjabi" };
            ViewBag.Languages = createLanguage;

            // Fetch teachers for selection
            var teachers = await _mediator.Send(new GetAllTeachersQuery());
            ViewBag.Teachers = teachers;

            _logger.Information("Languages and teachers fetched successfully.");
            return View();
        }

        /// <summary>
        /// Handles the submission of the Add Subject form.
        /// </summary>
        /// <param name="model">Command model containing the details of the subject to add.</param>
        [HttpPost]
        public async Task<IActionResult> Add(AddSubjectCommand model)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warning("Invalid model state detected in Add Subject form submission.");
                return View(model);
            }

            _logger.Information("Adding new subject with details: {@SubjectDetails}", model);

            var subjectId = await _mediator.Send(model);

            if (subjectId > 0)
            {
                _logger.Information("Subject added successfully with ID {SubjectId}.", subjectId);
                return RedirectToAction("Index");
            }
            else
            {
                _logger.Error("Failed to add subject.");
                ModelState.AddModelError("", "An error occurred while adding the subject.");
                return View(model);
            }
        }

        #endregion
    }
}
