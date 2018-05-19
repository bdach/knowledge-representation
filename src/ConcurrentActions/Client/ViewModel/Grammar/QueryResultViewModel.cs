using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.View.Grammar;

namespace Client.ViewModel.Grammar
{
    /// <summary>
    /// View model for <see cref="QueryResultView"/> which represents a logical negation.
    /// </summary>
    public class QueryResultViewModel
    {
        /// <summary>
        /// Query string representation
        /// </summary>
        public string Query { get; set; }
        /// <summary>
        /// Query result: true or false
        /// </summary>
        public bool Result { get; set; }

        public QueryResultViewModel(string query, bool result)
        {
            Query = query;
            Result = result;
        }
    }
}
