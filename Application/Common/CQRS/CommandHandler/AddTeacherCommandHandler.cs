using Application.Common.CQRS.Command;
using Domain.Context;
using MediatR;
using Serilog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers
{
    /// <summary>
    /// Handles the command to add a new teacher.
    /// </summary>
    public class AddTeacherCommandHandler : IRequestHandler<AddTeacherCommand, int>
    {
        private readonly SchoolContext _context;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor for AddTeacherCommandHandler.
        /// </summary>
        /// <param name="context">The DbContext for interacting with the database.</param>
        /// <param name="logger">Serilog logger for logging operations.</param>
        public AddTeacherCommandHandler(SchoolContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Public Methods

        /// <summary>
        /// Handles the request to add a new teacher to the database.
        /// </summary>
        /// <param name="request">The command containing teacher details.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The ID of the newly created teacher.</returns>
        public async Task<int> Handle(AddTeacherCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.Information("Processing AddTeacherCommand for teacher: {TeacherName}", request.Name);

                // Convert image file to base64 string if provided
                string base64Image = string.Empty;
                if (request.ImageFile != null && request.ImageFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await request.ImageFile.CopyToAsync(ms, cancellationToken);
                        var fileBytes = ms.ToArray();
                        base64Image = Convert.ToBase64String(fileBytes);
                    }
                }

                // Create and add the new teacher
                var teacher = new Teacher
                {
                    Name = request.Name,
                    Age = request.Age,
                    Image = base64Image,
                    Sex = request.Sex
                };

                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.Information("Teacher added successfully with ID: {TeacherId}", teacher.Id);
                return teacher.Id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while adding a new teacher.");
                throw;
            }
        }

        #endregion
    }
}
