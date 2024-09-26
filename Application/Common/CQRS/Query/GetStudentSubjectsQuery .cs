using MediatR;
using System.Collections.Generic;

namespace Application.Common.CQRS.Query
{
    #region Queries

    /// <summary>
    /// Query to retrieve the list of subjects and associated teacher details for a specific student.
    /// </summary>
    public class GetStudentSubjectsQuery : IRequest<List<SubjectTeacherView>>
    {
        /// <summary>
        /// Gets or sets the ID of the student for whom subjects are being queried.
        /// </summary>
        public int StudentId { get; set; }
    }

    #endregion

    #region View Models

    /// <summary>
    /// View model representing subject details along with associated teacher information.
    /// </summary>
    public class SubjectTeacherView
    {
        /// <summary>
        /// Gets or sets the name of the subject.
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Gets or sets the class to which the subject belongs.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the name of the teacher for the subject.
        /// </summary>
        public string TeacherName { get; set; }
    }

    #endregion
}
