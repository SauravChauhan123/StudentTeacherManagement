using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Common.CQRS.Command
{
    /// <summary>
    /// Command to add a new teacher.
    /// </summary>
    public class AddTeacherCommand : IRequest<int>
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the teacher.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the age of the teacher.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the image path for the teacher.
        /// </summary>
        public string Image { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the sex of the teacher.
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// Gets or sets the uploaded image file for the teacher.
        /// This property is not mapped to the database.
        /// </summary>
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        #endregion
    }
}
