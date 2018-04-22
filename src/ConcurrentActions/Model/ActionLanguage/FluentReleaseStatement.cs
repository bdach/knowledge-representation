using Model.Forms;

namespace Model.ActionLanguage
{
    public class FluentReleaseStatement
    {
        public Action Action { get; set; }
        public Fluent Fluent { get; set; }
        public IFormula Precondition { get; set; }

        public FluentReleaseStatement(Action action, Fluent fluent, IFormula precondition)
        {
            Action = action;
            Fluent = fluent;
            Precondition = precondition;
        }

        public FluentReleaseStatement(Action action, Fluent fluent) : this(action, fluent, Constant.Truth)
        {
        }

        public override string ToString()
        {
            var prefix = $"{Action} releases {Fluent}";
            var suffix = Precondition == Constant.Truth ? "" : $"if {Precondition}";
            return string.Join(" ", prefix, suffix);
        }
    }
}