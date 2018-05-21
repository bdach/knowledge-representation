using DynamicSystem.NewGeneration;
using Model;
using Model.ActionLanguage;
using Model.Forms;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = Model.Action;

namespace Test.NewGeneration
{
    class NewGenerationLightTest
    {
        private readonly Fluent _switchOne = new Fluent("switchOne");
        private readonly Fluent _switchTwo = new Fluent("switchTwo");
        private readonly Fluent _light = new Fluent("light");
        private HashSet<Fluent> _fluents;
        private ActionDomain _actionDomain;

        private readonly Action _toggleOne = new Action("toggleOne");
        private readonly Action _toggleTwo = new Action("toggleTwo");

        private State _stateOne;
        private State _stateTwo;
        
        [OneTimeSetUp]
        public void Init()
        {
            _actionDomain = new ActionDomain();
            //noninertial light
            _actionDomain.FluentSpecificationStatements.Add(new FluentSpecificationStatement(_light));
            _actionDomain.InitialValueStatements.Add(new InitialValueStatement(new Conjunction(new Literal(_switchOne), new Literal(_switchTwo))));
            _actionDomain.ConstraintStatements.Add(new ConstraintStatement(
                        new Equivalence(
                            new Literal(_light),
                            new Equivalence(new Literal(_switchOne), new Literal(_switchTwo))
                        )
                    ));

            _actionDomain.EffectStatements.Add(new EffectStatement(_toggleOne, new Literal(_switchOne), new Negation(new Literal(_switchOne))));
            _actionDomain.EffectStatements.Add(new EffectStatement(_toggleOne, new Negation(new Literal(_switchOne)), new Literal(_switchOne)));
            _actionDomain.EffectStatements.Add(new EffectStatement(_toggleTwo, new Literal(_switchTwo), new Negation(new Literal(_switchTwo))));
            _actionDomain.EffectStatements.Add(new EffectStatement(_toggleTwo, new Negation(new Literal(_switchTwo)), new Literal(_switchTwo)));


            _fluents = new HashSet<Fluent> { _light, _switchOne, _switchTwo };


            var fluentStatesOne = new Dictionary<Fluent, bool>() { { _switchOne, true }, { _switchTwo, true }, { _light, true } };
            var fluentStatesTwo = new Dictionary<Fluent, bool>() { { _switchOne, false }, { _switchTwo, false }, { _light, false } };

            _stateOne = new State(fluentStatesOne);
            _stateTwo = new State(fluentStatesTwo);
        }

        [Test]
        public void SwitchSecondLightWhenBothOn()
        {
            var newGenerator = new NewSetHelper(_actionDomain, _fluents);
            var literals = newGenerator.GetLiterals(_toggleTwo, _stateOne, _stateTwo);

            Assert.IsTrue(literals.Count() == 2);
            Assert.IsTrue(literals.Contains(new Literal(_switchOne, true)));
            Assert.IsTrue(literals.Contains(new Literal(_switchTwo, true)));
        }
    }
}
