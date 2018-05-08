using System.Collections.Generic;
using Model.QueryLanguage;

namespace DynamicSystem
{
    public class QueryResolution
    {
        public List<(AccessibilityQuery, bool)> AccessibilityQueryResults { get; }
        public List<(ExistentialExecutabilityQuery, bool)> ExistentialExecutabilityQueryResults { get; }
        public List<(GeneralExecutabilityQuery, bool)> GeneralExecutabilityQueryResults { get; }
        public List<(ExistentialValueQuery, bool)> ExistentialValueQueryResults { get; }
        public List<(GeneralValueQuery, bool)> GeneralValueQueryResults { get; }

        public QueryResolution()
        {
            AccessibilityQueryResults = new List<(AccessibilityQuery, bool)>();
            ExistentialExecutabilityQueryResults = new List<(ExistentialExecutabilityQuery, bool)>();
            GeneralExecutabilityQueryResults = new List<(GeneralExecutabilityQuery, bool)>();
            ExistentialValueQueryResults = new List<(ExistentialValueQuery, bool)>();
            GeneralValueQueryResults = new List<(GeneralValueQuery, bool)>();
        }
    }
}