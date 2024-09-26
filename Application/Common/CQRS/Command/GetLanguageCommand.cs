using Domain.Context;
using MediatR;
using System.Collections.Generic;

namespace Application.Common.CQRS.Command
{
    /// <summary>
    /// Command to retrieve a list of languages.
    /// </summary>
    public class GetLanguageCommand : IRequest<List<Language>>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the list of language names to filter the result.
        /// </summary>
        public List<string> Languages { get; set; }

        #endregion
    }
}
