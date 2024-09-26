using MediatR;
using System.Collections.Generic;

namespace Application.Common.CQRS.Command
{
    /// <summary>
    /// Command to add a new language or multiple languages.
    /// </summary>
    public class AddLanguageCommand : IRequest<int>
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the list of language names to be added.
        /// </summary>
        public List<string> Name { get; set; }

        #endregion
    }
}
