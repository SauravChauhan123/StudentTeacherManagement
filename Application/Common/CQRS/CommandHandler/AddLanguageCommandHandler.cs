using Application.Common.CQRS.Command;
using Domain.Context;
using MediatR;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.CQRS.CommandHandler
{
    /// <summary>
    /// Handles the command to add languages to the database.
    /// </summary>
    public class AddLanguagesCommandHandler : IRequestHandler<AddLanguageCommand, int>
    {
        private readonly SchoolContext _context;
        private readonly ILogger _logger;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AddLanguagesCommandHandler"/> class.
        /// </summary>
        /// <param name="context">The database context used for accessing the database.</param>
        /// <param name="logger">The Serilog logger for logging operations.</param>
        public AddLanguagesCommandHandler(SchoolContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        #endregion

        #region Handle Method

        /// <summary>
        /// Handles the command to add languages to the database.
        /// </summary>
        /// <param name="request">The command containing the list of language names to be added.</param>
        /// <param name="cancellationToken">The cancellation token for async operations.</param>
        /// <returns>The number of languages added to the database.</returns>
        public async Task<int> Handle(AddLanguageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Create a list of Language objects from the provided names
                var languages = request.Name.Select(name => new Language { Name = name })
                                           .ToList();

                // Log the number of languages to be added
                _logger.Information("Adding {LanguageCount} languages to the database.", languages.Count);

                // Add the range of languages to the DbContext
                _context.Language.AddRange(languages);

                // Save changes to the database
                var rowsAffected = await _context.SaveChangesAsync(cancellationToken);

                // Log the result of the save operation
                _logger.Information("Successfully added {RowsAffected} languages to the database.", rowsAffected);

                // Return the count of inserted languages
                return rowsAffected;
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.Error(ex, "An error occurred while adding languages to the database.");
                throw; // Re-throw the exception after logging it
            }
        }

        #endregion
    }
}
