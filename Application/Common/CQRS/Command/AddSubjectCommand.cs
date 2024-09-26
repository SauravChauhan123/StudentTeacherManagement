using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.CQRS.Command
{
    /// <summary>
    /// Command for adding a new subject with associated details.
    /// </summary>
    public class AddSubjectCommand : IRequest<int>
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the subject.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the class for which the subject is intended.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the list of languages in which the subject is available.
        /// </summary>
        public List<string> Languages { get; set; }

        /// <summary>
        /// Gets or sets the list of IDs of teachers who teach the subject.
        /// </summary>
        public List<int> TeacherIds { get; set; }

        #endregion
    }
}
