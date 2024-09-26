using Application.Common.CQRS.Command;
using Domain.Context;
using System.Collections.Generic;

namespace Infrastructure.ViewModels
{
    /// <summary>
    /// ViewModel for displaying a list of students with optional search functionality.
    /// </summary>
    public class StudentListViewModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets the search term used to filter the list of students.
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the list of students.
        /// </summary>
        public List<Student> Students { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentListViewModel"/> class.
        /// </summary>
        public StudentListViewModel()
        {
            Students = new List<Student>();
        }

        #endregion
    }
}
