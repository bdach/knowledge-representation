using System.Collections.Generic;
using Model;
using Model.ActionLanguage;
using Model.Forms;
using Model.QueryLanguage;

namespace Client.Global
{
    public class Scenario
    {
        public List<Action> Actions { get; set; }

        public List<CompoundAction> CompoundActions { get; set; }

        public List<Literal> Literals { get; set; }

        public List<Program> Programs { get; set; }

        public ActionDomain ActionDomain { get; set; }

        public QuerySet QuerySet { get; set; }

        public Scenario()
        {
            //Actions = new List<Action>();
            //CompoundActions = new List<CompoundAction>();
            //Literals = new List<Literal>();
            //Programs = new List<Program>();
            //ActionDomain = new ActionDomain();
            //QuerySet = new QuerySet();
        }
    }
}