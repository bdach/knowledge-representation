using System.Collections.Generic;
using Model;
using Model.Forms;

namespace Test.MinimizeNew.TestCases
{
    internal class TwoPaintersTransitionFunctionGenerationTestCase : ITransitionFunctionGenerationTestCase
    {
        public TransitionFunction ResZero => resZero;
        public Dictionary<(CompoundAction, State, State), HashSet<Literal>> NewSets => newSets;
        public TransitionFunction TransitionFunction
        {
            get
            {
                var transitionFunction = new TransitionFunction(CompoundAction.Values, State);
                foreach (var entry in res)
                    transitionFunction[entry.Key.Item1, entry.Key.Item2] = entry.Value;
                return transitionFunction;
            }
        }

        private static readonly Dictionary<string, Fluent> Fluent = new Dictionary<string, Fluent>
        {
            {"brushA", new Fluent("brushA")},
            {"brushB", new Fluent("brushB")}
        };

        private static readonly Dictionary<string, Action> Action = new Dictionary<string, Action>
        {
            {"TAKE_A", new Action("TAKE_A")},
            {"TAKE_B", new Action("TAKE_B")},
            {"PAINT", new Action("PAINT")}
        };

        private static readonly Dictionary<string, CompoundAction> CompoundAction =
            new Dictionary<string, CompoundAction>
            {
                {"{TAKE_A}", new CompoundAction(new[] {Action["TAKE_A"] })},
                {"{TAKE_B}", new CompoundAction(new[] {Action["TAKE_B"] })},
                {"{PAINT}", new CompoundAction(new[] {Action["PAINT"] })},
                {"{TAKE_A, TAKE_B}", new CompoundAction(new[] {Action["TAKE_A"], Action["TAKE_B"] })},
                {"{TAKE_A, PAINT}", new CompoundAction(new[] {Action["TAKE_A"], Action["PAINT"] })},
                {"{TAKE_B, PAINT}", new CompoundAction(new[] {Action["TAKE_B"], Action["PAINT"] })},
                {"{TAKE_A, TAKE_B, PAINT}", new CompoundAction(new[] {Action["TAKE_A"], Action["TAKE_B"], Action["PAINT"] })}
            };

        private static readonly State[] State =
        {
            new State(new Dictionary<Fluent, bool> {{Fluent["brushA"], false}, {Fluent["brushB"], false}}),
            new State(new Dictionary<Fluent, bool> {{Fluent["brushA"], true}, {Fluent["brushB"], false}}),
            new State(new Dictionary<Fluent, bool> {{Fluent["brushA"], false}, {Fluent["brushB"], true}})
        };

        private static readonly Dictionary<string, Literal> Literal = new Dictionary<string, Literal>
        {
            {"brushA", new Literal(Fluent["brushA"], false)},
            {"~brushA", new Literal(Fluent["brushA"], true)},
            {"brushB", new Literal(Fluent["brushB"], false)},
            {"~brushB", new Literal(Fluent["brushB"], true)}
        };

        private static TransitionFunction resZero =
            new TransitionFunction(CompoundAction.Values, State)
            {
                [CompoundAction["{TAKE_A}"], State[0]] = new HashSet<State> {State[1]},
                [CompoundAction["{TAKE_B}"], State[0]] = new HashSet<State> {State[2]},
                [CompoundAction["{PAINT}"], State[0]] = new HashSet<State> {State[0]},
                [CompoundAction["{TAKE_A, TAKE_B}"], State[0]] = new HashSet<State> {State[1], State[2]},
                [CompoundAction["{TAKE_A, PAINT}"], State[0]] = new HashSet<State> {State[1]},
                [CompoundAction["{TAKE_B, PAINT}"], State[0]] = new HashSet<State> {State[2]},
                [CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[0]] = new HashSet<State> {State[1], State[2]},

                [CompoundAction["{TAKE_A}"], State[1]] = new HashSet<State> {State[1]},
                [CompoundAction["{TAKE_B}"], State[1]] = new HashSet<State> {State[2]},
                [CompoundAction["{PAINT}"], State[1]] = new HashSet<State> {State[0], State[2]},
                [CompoundAction["{TAKE_A, TAKE_B}"], State[1]] = new HashSet<State> {State[1], State[2]},
                [CompoundAction["{TAKE_A, PAINT}"], State[1]] = new HashSet<State> {State[0], State[1], State[2]},
                [CompoundAction["{TAKE_B, PAINT}"], State[1]] = new HashSet<State> {State[2]},
                [CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[1]] = new HashSet<State> {State[1], State[2]},

                [CompoundAction["{TAKE_A}"], State[2]] = new HashSet<State> {State[1]},
                [CompoundAction["{TAKE_B}"], State[2]] = new HashSet<State> {State[2]},
                [CompoundAction["{PAINT}"], State[2]] = new HashSet<State> {State[0], State[1]},
                [CompoundAction["{TAKE_A, TAKE_B}"], State[2]] = new HashSet<State> {State[1], State[2]},
                [CompoundAction["{TAKE_A, PAINT}"], State[2]] = new HashSet<State> {State[1]},
                [CompoundAction["{TAKE_B, PAINT}"], State[2]] = new HashSet<State> {State[0], State[1], State[2]},
                [CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[2]] = new HashSet<State> {State[1], State[2]}
            };

        private static Dictionary<(CompoundAction, State, State), HashSet<Literal>> newSets =
            new Dictionary<(CompoundAction, State, State), HashSet<Literal>>
            {
                {(CompoundAction["{TAKE_A}"], State[0], State[0]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_B}"], State[0], State[0]), new HashSet<Literal> { }},
                {(CompoundAction["{PAINT}"], State[0], State[0]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, TAKE_B}"], State[0], State[0]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, PAINT}"], State[0], State[0]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_B, PAINT}"], State[0], State[0]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[0], State[0]), new HashSet<Literal> { }},

                {(CompoundAction["{TAKE_A}"], State[1], State[0]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_B}"], State[1], State[0]), new HashSet<Literal> { }},
                {(CompoundAction["{PAINT}"], State[1], State[0]), new HashSet<Literal> { Literal["brushA"] }},
                {(CompoundAction["{TAKE_A, TAKE_B}"], State[1], State[0]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, PAINT}"], State[1], State[0]), new HashSet<Literal> { Literal["brushA"] }},
                {(CompoundAction["{TAKE_B, PAINT}"], State[1], State[0]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[1], State[0]), new HashSet<Literal> { }},

                {(CompoundAction["{TAKE_A}"], State[2], State[0]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_B}"], State[2], State[0]), new HashSet<Literal> { }},
                {(CompoundAction["{PAINT}"], State[2], State[0]), new HashSet<Literal> { Literal["brushB"] }},
                {(CompoundAction["{TAKE_A, TAKE_B}"], State[2], State[0]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, PAINT}"], State[2], State[0]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_B, PAINT}"], State[2], State[0]), new HashSet<Literal> { Literal["brushB"] }},
                {(CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[2], State[0]), new HashSet<Literal> { }},


                {(CompoundAction["{TAKE_A}"], State[0], State[1]), new HashSet<Literal> { Literal["brushA"] }},
                {(CompoundAction["{TAKE_B}"], State[0], State[1]), new HashSet<Literal> { }},
                {(CompoundAction["{PAINT}"], State[0], State[1]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, TAKE_B}"], State[0], State[1]), new HashSet<Literal> { Literal["brushA"] }},
                {(CompoundAction["{TAKE_A, PAINT}"], State[0], State[1]), new HashSet<Literal> { Literal["brushA"] }},
                {(CompoundAction["{TAKE_B, PAINT}"], State[0], State[1]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[0], State[1]), new HashSet<Literal> { Literal["brushA"] }},

                {(CompoundAction["{TAKE_A}"], State[1], State[1]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_B}"], State[1], State[1]), new HashSet<Literal> { }},
                {(CompoundAction["{PAINT}"], State[1], State[1]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, TAKE_B}"], State[1], State[1]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, PAINT}"], State[1], State[1]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_B, PAINT}"], State[1], State[1]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[1], State[1]), new HashSet<Literal> { }},

                {(CompoundAction["{TAKE_A}"], State[2], State[1]), new HashSet<Literal> { Literal["brushA"], Literal["~brushB"] }},
                {(CompoundAction["{TAKE_B}"], State[2], State[1]), new HashSet<Literal> { }},
                {(CompoundAction["{PAINT}"], State[2], State[1]), new HashSet<Literal> { Literal["brushA"], Literal["brushB"] }},
                {(CompoundAction["{TAKE_A, TAKE_B}"], State[2], State[1]), new HashSet<Literal> { Literal["brushA"], Literal["brushB"] }},
                {(CompoundAction["{TAKE_A, PAINT}"], State[2], State[1]), new HashSet<Literal> { Literal["brushA"], Literal["~brushB"] }},
                {(CompoundAction["{TAKE_B, PAINT}"], State[2], State[1]), new HashSet<Literal> { Literal["brushA"], Literal["brushB"] }},
                {(CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[2], State[1]), new HashSet<Literal> { Literal["brushA"], Literal["brushB"] }},


                {(CompoundAction["{TAKE_A}"], State[0], State[2]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_B}"], State[0], State[2]), new HashSet<Literal> { Literal["brushB"] }},
                {(CompoundAction["{PAINT}"], State[0], State[2]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, TAKE_B}"], State[0], State[2]), new HashSet<Literal> { Literal["brushB"] }},
                {(CompoundAction["{TAKE_A, PAINT}"], State[0], State[2]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_B, PAINT}"], State[0], State[2]), new HashSet<Literal> { Literal["brushB"] }},
                {(CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[0], State[2]), new HashSet<Literal> { Literal["brushB"] }},

                {(CompoundAction["{TAKE_A}"], State[1], State[2]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_B}"], State[1], State[2]), new HashSet<Literal> { Literal["~brushA"], Literal["brushB"] }},
                {(CompoundAction["{PAINT}"], State[1], State[2]), new HashSet<Literal> { Literal["brushA"], Literal["brushB"] }},
                {(CompoundAction["{TAKE_A, TAKE_B}"], State[1], State[2]), new HashSet<Literal> { Literal["brushA"], Literal["brushB"] }},
                {(CompoundAction["{TAKE_A, PAINT}"], State[1], State[2]), new HashSet<Literal> { Literal["brushA"], Literal["brushB"] }},
                {(CompoundAction["{TAKE_B, PAINT}"], State[1], State[2]), new HashSet<Literal> { Literal["~brushA"], Literal["brushB"] }},
                {(CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[1], State[2]), new HashSet<Literal> { Literal["brushA"], Literal["brushB"] }},

                {(CompoundAction["{TAKE_A}"], State[2], State[2]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_B}"], State[2], State[2]), new HashSet<Literal> { }},
                {(CompoundAction["{PAINT}"], State[2], State[2]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, TAKE_B}"], State[2], State[2]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, PAINT}"], State[2], State[2]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_B, PAINT}"], State[2], State[2]), new HashSet<Literal> { }},
                {(CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[2], State[2]), new HashSet<Literal> { }},
            };

        private static Dictionary<(CompoundAction, State), HashSet<State>> res =
            new Dictionary<(CompoundAction, State), HashSet<State>>
            {
                {(CompoundAction["{TAKE_A}"], State[0]), new HashSet<State> {State[1]}},
                {(CompoundAction["{TAKE_B}"], State[0]), new HashSet<State> {State[2]}},
                {(CompoundAction["{PAINT}"], State[0]), new HashSet<State> {State[0]}},
                {(CompoundAction["{TAKE_A, TAKE_B}"], State[0]), new HashSet<State> {State[1], State[2]}},
                {(CompoundAction["{TAKE_A, PAINT}"], State[0]), new HashSet<State> {State[1]}},
                {(CompoundAction["{TAKE_B, PAINT}"], State[0]), new HashSet<State> {State[2]}},
                {(CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[0]), new HashSet<State> {State[1], State[2]}},

                {(CompoundAction["{TAKE_A}"], State[1]), new HashSet<State> {State[1]}},
                {(CompoundAction["{TAKE_B}"], State[1]), new HashSet<State> {State[2]}},
                {(CompoundAction["{PAINT}"], State[1]), new HashSet<State> {State[0]}},
                {(CompoundAction["{TAKE_A, TAKE_B}"], State[1]), new HashSet<State> {State[1]}},
                {(CompoundAction["{TAKE_A, PAINT}"], State[1]), new HashSet<State> {State[1]}},
                {(CompoundAction["{TAKE_B, PAINT}"], State[1]), new HashSet<State> {State[2]}},
                {(CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[1]), new HashSet<State> {State[1]}},

                {(CompoundAction["{TAKE_A}"], State[2]), new HashSet<State> {State[1]}},
                {(CompoundAction["{TAKE_B}"], State[2]), new HashSet<State> {State[2]}},
                {(CompoundAction["{PAINT}"], State[2]), new HashSet<State> {State[0]}},
                {(CompoundAction["{TAKE_A, TAKE_B}"], State[2]), new HashSet<State> {State[2]}},
                {(CompoundAction["{TAKE_A, PAINT}"], State[2]), new HashSet<State> {State[1]}},
                {(CompoundAction["{TAKE_B, PAINT}"], State[2]), new HashSet<State> {State[2]}},
                {(CompoundAction["{TAKE_A, TAKE_B, PAINT}"], State[2]), new HashSet<State> {State[2]}}
            };
    }
}
