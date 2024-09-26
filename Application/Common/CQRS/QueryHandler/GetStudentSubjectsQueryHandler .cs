using Application.Common.CQRS.Query;
using Domain.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.CQRS.QueryHandler
{
    /// <summary>
    /// Handles the query to get the list of subjects and their respective teachers for a specific student.
    /// </summary>
    public class GetStudentSubjectsQueryHandler : IRequestHandler<GetStudentSubjectsQuery, List<SubjectTeacherView>>
    {
        private readonly SchoolContext _context;
        private readonly ILogger _logger;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStudentSubjectsQueryHandler"/> class.
        /// </summary>
        /// <param name="context">The DbContext used to access the database.</param>
        /// <param name="logger">Serilog logger for logging operations.</param>
        public GetStudentSubjectsQueryHandler(SchoolContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        #endregion

        #region IRequestHandler Implementation

        /// <summary>
        /// Handles the query to retrieve subjects and teachers for a specific student.
        /// </summary>
        /// <param name="request">The query containing the student ID.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A list of <see cref="SubjectTeacherView"/> containing subject and teacher details.</returns>
        public async Task<List<SubjectTeacherView>> Handle(GetStudentSubjectsQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Handling GetStudentSubjectsQuery for StudentId {StudentId}.", request.StudentId);

            var subjectTeacherViewModel = new List<SubjectTeacherView>();

            try
            {
                // Retrieve student data
                var studentData = await _context.Students
                    .Where(x => x.Id == request.StudentId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (studentData == null)
                {
                    _logger.Warning("Student with ID {StudentId} not found.", request.StudentId);
                    return subjectTeacherViewModel;
                }

                // Retrieve associated teacher IDs
                var teacherIds = await _context.StudentTeacher
                    .Where(x => x.StudentId == request.StudentId)
                    .Select(x => x.TeacherId)
                    .ToListAsync(cancellationToken);

                // Retrieve teacher details
                foreach (var teacherId in teacherIds)
                {
                    var teacher = await _context.Teachers
                        .Include(x => x.Subjects)
                        .Where(x => x.Subjects.Any(s => s.Class == studentData.Class))
                        .FirstOrDefaultAsync(x => x.Id == teacherId, cancellationToken);

                    if (teacher != null)
                    {
                        subjectTeacherViewModel.Add(new SubjectTeacherView
                        {
                            Class = studentData.Class,
                            SubjectName = teacher.Subjects.FirstOrDefault()?.Name,
                            TeacherName = teacher.Name
                        });

                        _logger.Information("Added SubjectTeacherView for TeacherId {TeacherId}.", teacherId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while handling GetStudentSubjectsQuery for StudentId {StudentId}.", request.StudentId);
                throw;
            }

            _logger.Information("Successfully retrieved {Count} SubjectTeacherView items for StudentId {StudentId}.", subjectTeacherViewModel.Count, request.StudentId);
            return subjectTeacherViewModel;
        }

        #endregion
    }
}
