using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Common.CQRS.Command
{
    /// <summary>
    /// Command to add a new student.
    /// </summary>
    public class AddStudentCommand : IRequest<int>
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the student.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the age of the student.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the image of the student as a string.
        /// </summary>
        public string Image { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the class of the student.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the roll number of the student.
        /// </summary>
        public int RollNumber { get; set; }

        /// <summary>
        /// Gets or sets the image file uploaded by the user.
        /// This property is not mapped to the database.
        /// </summary>
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        /// <summary>
        /// Gets or sets the list of teacher IDs associated with the student.
        /// </summary>
        public List<string> TeacherId { get; set; }

        #endregion
    }
}
