using Application.Common.CQRS.Command;
using Domain.Context;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class AddStudentCommandHandler : IRequestHandler<AddStudentCommand, int>
{
    private readonly SchoolContext _context;
    private readonly ILogger _logger;

    /// <summary>
    /// Constructor for AddStudentCommandHandler.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="logger">Serilog logger for logging operations.</param>
    public AddStudentCommandHandler(SchoolContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }

    #region Handle Method

    /// <summary>
    /// Handles the AddStudentCommand to add a new student to the database.
    /// </summary>
    /// <param name="request">The command containing the student details.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>The ID of the newly created student.</returns>
    public async Task<int> Handle(AddStudentCommand request, CancellationToken cancellationToken)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
        {
            try
            {
                // Convert image file to base64 string
                string base64Image = "";
                if (request.ImageFile != null && request.ImageFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await request.ImageFile.CopyToAsync(ms, cancellationToken);
                        var fileBytes = ms.ToArray();
                        base64Image = Convert.ToBase64String(fileBytes);
                    }
                }

                // Create new student entity
                var student = new Student
                {
                    Name = request.Name,
                    Age = request.Age,
                    Image = base64Image,
                    Class = request.Class,
                    RollNumber = request.RollNumber
                };

                // Check if the roll number is already assigned in the given class
                if (_context.Students.Any(x => x.Class == request.Class && x.RollNumber == request.RollNumber))
                {
                    _logger.Warning("Roll number {RollNumber} already exists in class {Class}.", request.RollNumber, request.Class);
                    return 0;
                }

                // Add new student to the database
                _context.Students.Add(student);
                await _context.SaveChangesAsync(cancellationToken); // Save first to get the student ID

                // Add student-teacher relationships
                var studentTeacher = request.TeacherId.Select(x => new StudentTeacher
                {
                    StudentId = student.Id,
                    TeacherId = Convert.ToInt16(x)
                }).ToList();

                _context.StudentTeacher.AddRange(studentTeacher);
                await _context.SaveChangesAsync(cancellationToken);

                // Commit the transaction
                await transaction.CommitAsync(cancellationToken);

                _logger.Information("Student created successfully with ID {StudentId}.", student.Id);
                return student.Id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while adding student.");
                await transaction.RollbackAsync(cancellationToken); // Rollback the transaction on error
                throw;
            }
        }
    }

    #endregion
}
