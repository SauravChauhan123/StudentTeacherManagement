using Application.Common.CQRS.Query;
using System.Collections.Generic;

namespace Infrastructure.ViewModels
{
    /// <summary>
    /// Represents the view model containing details about subjects and teachers for a student.
    /// </summary>
    public class SubjectTeacherViewModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the list of subject and teacher details.
        /// </summary>
        public List<SubjectTeacherView> TeachersDetails { get; set; }

        #endregion
    }
}
