using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel.Grammar
{
    public class QueryResultViewModel
    {

        public string Query { get; set; }
        public bool Result { get; set; }

        public QueryResultViewModel(string query, bool result)
        {
            Query = query;
            Result = result;
        }
    }
}
