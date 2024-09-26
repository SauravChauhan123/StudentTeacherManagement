using Application.Common.CQRS.Command;
using Domain.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.CQRS.CommandHandler
{
    /// <summary>
    /// Handles the retrieval of students based on search criteria.
    /// </summary>
    public class GetStudentsCommandHandler : IRequestHandler<GetStudentCommand, List<Student>>
    {
        #region Fields

        private readonly SchoolContext _context;
        private readonly ILogger _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStudentsCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing students.</param>
        /// <param name="logger">Serilog logger for logging operations.</param>
        public GetStudentsCommandHandler(SchoolContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the request to retrieve students based on the search criteria.
        /// </summary>
        /// <param name="request">The command containing search criteria for retrieving students.</param>
        /// <param name="cancellationToken">Cancellation token for the async operation.</param>
        /// <returns>A list of students matching the search criteria.</returns>
        public async Task<List<Student>> Handle(GetStudentCommand request, CancellationToken cancellationToken)
        {
            _logger.Information("Handling GetStudentCommand with SearchTerm '{SearchTerm}' and Class '{Class}'.", request.SearchTerm, request.Class);

            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                _logger.Information("Filtering students by SearchTerm '{SearchTerm}'.", request.SearchTerm);
                query = query.Where(s => s.Name.Contains(request.SearchTerm));
            }

            if (!string.IsNullOrEmpty(request.Class))
            {
                _logger.Information("Filtering students by Class '{Class}'.", request.Class);
                query = query.Where(s => s.Class == request.Class);
            }

            var students = await query.ToListAsync(cancellationToken);
            _logger.Information("Retrieved {StudentCount} students.", students.Count);

            return students;
        }

        #endregion
    }
}
