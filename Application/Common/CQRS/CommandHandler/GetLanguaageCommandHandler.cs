using Application.Common.CQRS.Command;
using Domain.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.CQRS.CommandHandler
{
    /// <summary>
    /// Handles requests to retrieve a list of languages from the database.
    /// </summary>
    public class GetLanguageCommandHandler : IRequestHandler<GetLanguageCommand, List<Language>>
    {
        private readonly SchoolContext _context;
        private readonly ILogger _logger;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLanguageCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The database context for accessing the School database.</param>
        /// <param name="logger">Serilog logger for logging operations.</param>
        public GetLanguageCommandHandler(SchoolContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Handles the command to retrieve a list of languages from the database.
        /// </summary>
        /// <param name="request">The request containing the command data.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of languages.</returns>
        public async Task<List<Language>> Handle(GetLanguageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("Handling GetLanguageCommand at {Timestamp}", DateTime.UtcNow);
                var languages = await _context.Language.ToListAsync(cancellationToken);
                _logger.Information("Successfully retrieved {LanguageCount} languages at {Timestamp}", languages.Count, DateTime.UtcNow);
                return languages;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while handling GetLanguageCommand at {Timestamp}", DateTime.UtcNow);
                throw;
            }
        }

        #endregion
    }
}
