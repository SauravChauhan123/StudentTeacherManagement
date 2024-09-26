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
    #region Query

    /// <summary>
    /// Query to fetch all teachers.
    /// </summary>
    public class GetAllTeachersQuery : IRequest<List<TeacherDto>>
    {
    }

    #endregion

    #region Data Transfer Object (DTO)

    /// <summary>
    /// Data Transfer Object (DTO) to return only the needed teacher data.
    /// </summary>
    public class TeacherDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    #endregion

    #region Handler

    /// <summary>
    /// Handler to process the GetAllTeachersQuery.
    /// </summary>
    public class GetAllTeachersQueryHandler : IRequestHandler<GetAllTeachersQuery, List<TeacherDto>>
    {
        private readonly SchoolContext _context;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor for GetAllTeachersQueryHandler.
        /// </summary>
        /// <param name="context">The DbContext for accessing the database.</param>
        /// <param name="logger">Serilog logger for logging operations.</param>
        public GetAllTeachersQueryHandler(SchoolContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Handles the GetAllTeachersQuery and returns a list of TeacherDto.
        /// </summary>
        /// <param name="request">The query request.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A list of TeacherDto.</returns>
        public async Task<List<TeacherDto>> Handle(GetAllTeachersQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Handling GetAllTeachersQuery");

            try
            {
                var teachers = await _context.Teachers
                    .Select(t => new TeacherDto
                    {
                        Id = t.Id,
                        Name = t.Name
                    })
                    .ToListAsync(cancellationToken);

                _logger.Information("Successfully retrieved {TeacherCount} teachers.", teachers.Count);
                return teachers;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while retrieving teachers.");
                throw;
            }
        }

    }

    #endregion
}
