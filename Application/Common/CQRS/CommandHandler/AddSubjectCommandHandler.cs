using Application.Common.CQRS.Command;
using Domain.Context;
using MediatR;
using Serilog;

namespace Application.CommandHandlers
{
    /// <summary>
    /// Handler for adding a new subject.
    /// </summary>
    public class AddSubjectCommandHandler : IRequestHandler<AddSubjectCommand, int>
    {
        private readonly SchoolContext _context;
        private readonly ILogger _logger;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AddSubjectCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing the School database.</param>
        /// <param name="logger">The logger for logging operations.</param>
        public AddSubjectCommandHandler(SchoolContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Handles the request to add a new subject.
        /// </summary>
        /// <param name="request">The command containing the details of the subject to add.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>The ID of the newly created subject.</returns>
        public async Task<int> Handle(AddSubjectCommand request, CancellationToken cancellationToken)
        {
            _logger.Information("Handling AddSubjectCommand for subject: {SubjectName}", request.Name);

            var subject = new Subject
            {
                Name = request.Name,
                Class = request.Class,
                Languages = request.Languages
            };

            foreach (var teacherId in request.TeacherIds)
            {
                var teacher = await _context.Teachers.FindAsync(teacherId);
                if (teacher != null)
                {
                    subject.Teachers.Add(teacher);
                }
                else
                {
                    _logger.Warning("Teacher with ID {TeacherId} not found and was not added to the subject.", teacherId);
                }
            }

            _context.Subjects.Add(subject);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                _logger.Information("Subject created successfully with ID {SubjectId}.", subject.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while saving the new subject.");
                throw;
            }

            return subject.Id;
        }

        #endregion
    }
}
