using Domain.Context;
using MediatR;
using System.Collections.Generic;

namespace Application.Common.CQRS.Command
{
    /// <summary>
    /// Command to retrieve a list of students based on search term and class.
    /// </summary>
    public class GetStudentCommand : IRequest<List<Student>>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the search term to filter students by name.
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the class to filter students by their class.
        /// </summary>
        public string Class { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStudentCommand"/> class.
        /// </summary>
        /// <param name="searchTerm">The term to search for students.</param>
        /// <param name="class">The class to filter students by.</param>
        public GetStudentCommand(string searchTerm, string @class)
        {
            SearchTerm = searchTerm;
            Class = @class;
        }

        #endregion
    }
}
