using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Model.ActionLanguage;
using Model.Forms;
using Model.QueryLanguage;

namespace DynamicSystem.Grammar
{
    public static class DynamicSystemParserUtils
    {
        public static ActionDomain ParseActionDomain(string input)
        {
            var parser = new DynamicSystemParser(
                new CommonTokenStream(new DynamicSystemLexer(new AntlrInputStream(input))));
            return parser.actionDomain().Accept(new ActionDomainParsingVisitor()) as ActionDomain;
        }

        public static IFormula ParseFormula(string input)
        {
            var parser = new DynamicSystemParser(
                new CommonTokenStream(new DynamicSystemLexer(new AntlrInputStream(input))));
            return parser.actionDomain().Accept(new FormulaParsingVisitor()) ;
        }

        public static QuerySet ParseQuerySet(string input)
        {
            var parser = new DynamicSystemParser(
                new CommonTokenStream(new DynamicSystemLexer(new AntlrInputStream(input))));
            return parser.querySet().Accept(new QuerySetParsingVisitor()) as QuerySet;
        }

    }
}