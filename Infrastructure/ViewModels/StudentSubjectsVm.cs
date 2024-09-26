using Domain.Context;
using System.Collections.Generic;

namespace Infrastructure.ViewModels
{
    /// <summary>
    /// Represents a view model for displaying student subjects.
    /// </summary>
    public class StudentSubjectsVm
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the list of subjects associated with the student.
        /// </summary>
        public List<Subject> Subjects { get; set; }

        #endregion
    }
}
