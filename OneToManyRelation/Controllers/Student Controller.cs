using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Infrastructure.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OneToManyRelation.Controllers
{
    /// <summary>
    /// Handles student-related operations such as creating, listing, and retrieving teachers and subjects.
    /// </summary>
    public class StudentController : Controller
    {
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        /// <summary>
        /// Constructor for StudentController.
        /// </summary>
        /// <param name="mediator">Mediator for sending commands and queries.</param>
        /// <param name="logger">Serilog logger for logging operations.</param>
        public StudentController(IMediator mediator, Serilog.ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Create Student

        /// <summary>
        /// Displays the student creation form.
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles the student creation POST request.
        /// </summary>
        /// <param name="command">Command containing the student details to add.</param>
        [HttpPost]
        public async Task<IActionResult> Create(AddStudentCommand command)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warning("Invalid model state for AddStudentCommand.");
                return View(command);
            }

            var id = await _mediator.Send(command);
            if (id == 0)
            {
                _logger.Error("Failed to create a new student.");
                ModelState.AddModelError("", "An error occurred while creating the student.");
                return View(command);
            }

            _logger.Information("Student created successfully with ID {StudentId}.", id);
            return RedirectToAction("Index");
        }

        #endregion

        #region List Students

        /// <summary>
        /// Displays the list of students with optional search and filter criteria.
        /// </summary>
        /// <param name="searchTerm">Search term to filter students by name.</param>
        /// <param name="studentClass">Class to filter students by class.</param>
        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm, string studentClass)
        {
            var query = new GetStudentCommand(searchTerm, studentClass);
            var students = await _mediator.Send(query);

            var viewModel = new StudentListViewModel
            {
                SearchTerm = searchTerm,
                Students = students
            };

            _logger.Information("Listing students with search term '{SearchTerm}' and class '{StudentClass}'.", searchTerm, studentClass);
            return View(viewModel);
        }

        #endregion

        #region Get Teachers By Class

        /// <summary>
        /// Retrieves the list of teachers associated with a specific class.
        /// </summary>
        /// <param name="className">The class name to filter teachers.</param>
        [HttpGet]
        public async Task<IActionResult> GetTeachersByClass(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                _logger.Warning("Class name is null or empty in GetTeachersByClass.");
                return Json(new List<TeacherDto>());
            }

            var query = new GetAllTeacherByClassQuery { ClassName = className };
            var teachers = await _mediator.Send(query);

            _logger.Information("Retrieved {TeacherCount} teachers for class '{ClassName}'.", teachers.Count(), className);
            return Json(teachers);
        }

        #endregion

        #region List Subjects for Student

        /// <summary>
        /// Displays the list of subjects and teachers for a specific student.
        /// </summary>
        /// <param name="studentId">The ID of the student to retrieve subjects and teachers for.</param>
        [HttpGet]
        public async Task<IActionResult> SubjectList(int studentId)
        {
            var query = new GetStudentSubjectsQuery { StudentId = studentId };
            var data = await _mediator.Send(query);

            var viewModel = new SubjectTeacherViewModel
            {
                TeachersDetails = data
            };

            _logger.Information("Retrieved subjects and teachers for student with ID {StudentId}.", studentId);
            return View(viewModel);
        }

        #endregion
    }
}
