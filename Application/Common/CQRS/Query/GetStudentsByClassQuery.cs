using Domain.Context;
using MediatR;
using Serilog;
using System.Collections.Generic;

namespace Application.Common.CQRS.Query
{
    /// <summary>
    /// Query to retrieve a list of students by class.
    /// </summary>
    public class GetStudentsByClassQuery : IRequest<List<Student>>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the class name for which to retrieve students.
        /// </summary>
        public string Class { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStudentsByClassQuery"/> class.
        /// </summary>
        /// <param name="class">The class name to filter students by.</param>
        public GetStudentsByClassQuery(string @class)
        {
            Class = @class;
        }

        #endregion
    }
}
