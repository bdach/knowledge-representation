namespace Model.ActionLanguage
{
    public class FluentSpecificationStatement
    {
        public Fluent Fluent { get; set; }

        public FluentSpecificationStatement(Fluent fluent)
        {
            Fluent = fluent;
        }

        public override string ToString()
        {
            return $"noninertial {Fluent}";
        }
    }
}