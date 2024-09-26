using Domain.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.CQRS.Query
{
    #region Queries

    /// <summary>
    /// Query to get all teachers by class name.
    /// </summary>
    public class GetAllTeacherByClassQuery : IRequest<List<TeacherDto>>
    {
        public string ClassName { get; set; }
    }

    #endregion

    #region Handlers

    /// <summary>
    /// Handler for the GetAllTeacherByClassQuery query.
    /// </summary>
    public class GetAllTeacherByClassQueryHandler : IRequestHandler<GetAllTeacherByClassQuery, List<TeacherDto>>
    {
        private readonly SchoolContext _context;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor for GetAllTeacherByClassQueryHandler.
        /// </summary>
        /// <param name="context">The database context for accessing school data.</param>
        /// <param name="logger">Serilog logger for logging operations.</param>
        public GetAllTeacherByClassQueryHandler(SchoolContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Handles the GetAllTeacherByClassQuery request to retrieve teachers by class name.
        /// </summary>
        /// <param name="request">The query request containing the class name.</param>
        /// <param name="cancellationToken">Token to observe while awaiting the task.</param>
        /// <returns>A list of TeacherDto objects matching the specified class name.</returns>
        public async Task<List<TeacherDto>> Handle(GetAllTeacherByClassQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Handling GetAllTeacherByClassQuery for class '{ClassName}'", request.ClassName);

            var teachers = await _context.Teachers
                .Where(t => t.Subjects.Any(s => s.Class == request.ClassName))
                .Select(t => new TeacherDto
                {
                    Name = t.Name,
                    Id = t.Id
                })
                .ToListAsync(cancellationToken);

            _logger.Information("Retrieved {TeacherCount} teachers for class '{ClassName}'", teachers.Count, request.ClassName);

            return teachers;
        }
    }

    #endregion
}
