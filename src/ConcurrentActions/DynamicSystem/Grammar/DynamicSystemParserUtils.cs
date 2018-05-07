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
    /// <summary>
    /// Utility class which exposes methods for parsing <see cref="ActionDomain"/>, <see cref="IFormula"/>, <see cref="QuerySet"/>
    /// Following symbols are used to define formulas:
    /// TRUTH: 'T';
    /// FALSITY: 'F';
    /// NEGATION: '~';
    /// IMPLICATION: '-&gt;';
    /// EQUIVALENCE: '&lt;-&gt;';
    /// CONJUNCTION: '&amp;';
    /// ALTERNATIVE: '|';
    /// </summary>
    public static class DynamicSystemParserUtils
    {
        /// <summary>
        /// Creates <see cref="ActionDomain"/> instance from a string
        /// </summary>
        /// <param name="input">Statements</param>
        /// <returns>Parsed instance of <see cref="ActionDomain"/></returns>
        public static ActionDomain ParseActionDomain(string input)
        {
            var parser = CreateParser(input);
            return parser.actionDomain().Accept(new ActionDomainParsingVisitor()) as ActionDomain;
        }

        /// <summary>
        /// Creates <see cref="IFormula"/> instance from a string
        /// </summary>
        /// <param name="input">Formula to parse</param>
        /// <returns>Parsed instance of <see cref="IFormula"/></returns>
        public static IFormula ParseFormula(string input)
        {
            var parser = CreateParser(input);
            return parser.actionDomain().Accept(new FormulaParsingVisitor());
        }

        /// <summary>
        /// Creates <see cref="QuerySet"/> instance from a string
        /// </summary>
        /// <param name="input">Queries</param>
        /// <returns>Parsed instance of <see cref="QuerySet"/></returns>
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

        /// <summary>
        /// Parser error listener that throws <see cref="ParseCanceledException"/> on any error
        /// </summary>
        private class ThrowingErrorListener : BaseErrorListener, IAntlrErrorListener<int>
        {
            public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line,
                int charPositionInLine, string msg,
                RecognitionException e)
            {
                throw new ParseCanceledException("line " + line + ":" + charPositionInLine + " " + msg);
            }

            public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine,
                string msg,
                RecognitionException e)
            {
                throw new ParseCanceledException("line " + line + ":" + charPositionInLine + " " + msg);
            }
        }
    }
}