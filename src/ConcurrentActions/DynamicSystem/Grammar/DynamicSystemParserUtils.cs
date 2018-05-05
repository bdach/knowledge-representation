using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Model.ActionLanguage;
using Model.Forms;
using Model.QueryLanguage;

namespace DynamicSystem.Grammar
{
    public static class DynamicSystemParserUtils
    {
        public static ActionDomain ParseActionDomain(string input)
        {
            var parser = CreateParser(input);
            return parser.actionDomain().Accept(new ActionDomainParsingVisitor()) as ActionDomain;
        }

        public static IFormula ParseFormula(string input)
        {
            var parser = CreateParser(input);
            return parser.actionDomain().Accept(new FormulaParsingVisitor());
        }

        public static QuerySet ParseQuerySet(string input)
        {
            var parser = CreateParser(input);
            return parser.querySet().Accept(new QuerySetParsingVisitor()) as QuerySet;
        }

        private static DynamicSystemParser CreateParser(string input)
        {
            var lexer = new DynamicSystemLexer(
                new AntlrInputStream(input)
            );
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(ErrorListener);
            var parser = new DynamicSystemParser(
                new CommonTokenStream(
                    lexer
                )
            );
            parser.RemoveErrorListeners();
            parser.AddErrorListener(ErrorListener);
            return parser;
        }

        private static ThrowingErrorListener ErrorListener { get; } = new ThrowingErrorListener();

        private class ThrowingErrorListener : BaseErrorListener, IAntlrErrorListener<int>
        {
            public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg,
                RecognitionException e)
            {
                throw new ParseCanceledException("line " + line + ":" + charPositionInLine + " " + msg);
            }

            public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg,
                RecognitionException e)
            {
                throw new ParseCanceledException("line " + line + ":" + charPositionInLine + " " + msg);
            }
        }
    }
}