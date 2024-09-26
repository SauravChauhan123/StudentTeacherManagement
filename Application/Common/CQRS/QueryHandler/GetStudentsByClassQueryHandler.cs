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
    /// Handles queries to retrieve students based on their class.
    /// </summary>
    public class GetStudentsByClassQueryHandler : IRequestHandler<GetStudentsByClassQuery, List<Student>>
    {
        #region Fields

        private readonly SchoolContext _context;
        private readonly ILogger _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for GetStudentsByClassQueryHandler.
        /// </summary>
        /// <param name="context">Database context for querying students.</param>
        /// <param name="logger">Serilog logger for logging operations.</param>
        public GetStudentsByClassQueryHandler(SchoolContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the query to get a list of students based on the specified class.
        /// </summary>
        /// <param name="request">Query containing the class filter.</param>
        /// <param name="cancellationToken">Cancellation token for asynchronous operations.</param>
        /// <returns>A list of students in the specified class.</returns>
        public async Task<List<Student>> Handle(GetStudentsByClassQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Handling GetStudentsByClassQuery for class '{ClassName}'.", request.Class);

            var students = await _context.Students
                .Where(s => s.Class == request.Class)
                .ToListAsync(cancellationToken);

            _logger.Information("Retrieved {StudentCount} students for class '{ClassName}'.", students.Count, request.Class);

            return students;
        }

        #endregion
    }
}
