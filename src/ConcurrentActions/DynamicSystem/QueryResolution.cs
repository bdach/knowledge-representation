using System.Collections.Generic;
using Model.QueryLanguage;

namespace DynamicSystem
{
    public class QueryResolution
    {
        public Dictionary<AccessibilityQuery, bool> AccessibilityQueryResults { get; }
        public Dictionary<ExistentialExecutabilityQuery, bool> ExistentialExecutabilityQueryResults { get; }
        public Dictionary<GeneralExecutabilityQuery, bool> GeneralExecutabilityQueryResults { get; }
        public Dictionary<ExistentialValueQuery, bool> ExistentialValueQueryResults { get; }
        public Dictionary<GeneralValueQuery, bool> GeneralValueQueryResults { get; }

        public QueryResolution()
        {
            AccessibilityQueryResults = new Dictionary<AccessibilityQuery, bool>();
            ExistentialExecutabilityQueryResults = new Dictionary<ExistentialExecutabilityQuery, bool>();
            GeneralExecutabilityQueryResults = new Dictionary<GeneralExecutabilityQuery, bool>();
            ExistentialValueQueryResults = new Dictionary<ExistentialValueQuery, bool>();
            GeneralValueQueryResults = new Dictionary<GeneralValueQuery, bool>();
        }
    }
}