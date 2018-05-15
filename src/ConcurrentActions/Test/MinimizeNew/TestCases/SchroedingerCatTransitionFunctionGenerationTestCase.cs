using System.Collections.Generic;
using DynamicSystem.NewGeneration;
using Model;
using Model.Forms;

namespace Test.MinimizeNew.TestCases
{
    internal class SchroedingerCatTransitionFunctionGenerationTestCase : ITransitionFunctionGenerationTestCase
    {
        public TransitionFunction ResZero => resZero;
        public NewSetMapping NewSets => newSets;
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
            {"alive", new Fluent("alive")},
            {"purring", new Fluent("purring")}
        };

        private static readonly Dictionary<string, Action> Action = new Dictionary<string, Action>
        {
            {"PEEK", new Action("PEEK")},
            {"PET", new Action("PET")}
        };

        private static readonly Dictionary<string, CompoundAction> CompoundAction =
            new Dictionary<string, CompoundAction>
            {
                {"{PEEK}", new CompoundAction(new[] {Action["PEEK"]})},
                {"{PET}", new CompoundAction(new[] {Action["PET"]})},
                {"{PEEK, PET}", new CompoundAction(new[] {Action["PEEK"], Action["PET"]})}
            };

        private static readonly State[] State =
        {
            new State(new Dictionary<Fluent, bool> {{Fluent["alive"], true}, {Fluent["purring"], true}}),
            new State(new Dictionary<Fluent, bool> {{Fluent["alive"], true}, {Fluent["purring"], false}}),
            new State(new Dictionary<Fluent, bool> {{Fluent["alive"], false}, {Fluent["purring"], false}})
        };

        private static readonly Dictionary<string, Literal> Literal = new Dictionary<string, Literal>
        {
            {"alive", new Literal(Fluent["alive"], false)},
            {"~alive", new Literal(Fluent["alive"], true)},
            {"purring", new Literal(Fluent["purring"], false)},
            {"~purring", new Literal(Fluent["purring"], true)}
        };

        private static TransitionFunction resZero =
            new TransitionFunction(CompoundAction.Values, State)
            {
                [CompoundAction["{PEEK}"], State[0]] = new HashSet<State> {State[0], State[1], State[2]},
                [CompoundAction["{PET}"], State[0]] = new HashSet<State> {State[0]},
                [CompoundAction["{PEEK, PET}"], State[0]] = new HashSet<State> {State[0], State[1], State[2]},

                [CompoundAction["{PEEK}"], State[1]] = new HashSet<State> {State[0], State[1], State[2]},
                [CompoundAction["{PET}"], State[1]] = new HashSet<State> {State[0]},
                [CompoundAction["{PEEK, PET}"], State[1]] = new HashSet<State> {State[0], State[1], State[2]},

                [CompoundAction["{PEEK}"], State[2]] = new HashSet<State> {State[2]},
                [CompoundAction["{PET}"], State[2]] =  new HashSet<State> {State[2]},
                [CompoundAction["{PEEK, PET}"], State[2]] = new HashSet<State> {State[2]},
            };

        private static NewSetMapping newSets =
            new NewSetMapping
            {
                [CompoundAction["{PEEK}"], State[0], State[0]] = new HashSet<Literal> { Literal["alive"], Literal["purring"] },
                [CompoundAction["{PEEK}"], State[0], State[1]] = new HashSet<Literal> { Literal["alive"], Literal["~purring"] },
                [CompoundAction["{PEEK}"], State[0], State[2]] = new HashSet<Literal> { Literal["~alive"], Literal["~purring"] },

                [CompoundAction["{PET}"], State[0], State[0]] = new HashSet<Literal> { },
                [CompoundAction["{PET}"], State[0], State[1]] = new HashSet<Literal> { },
                [CompoundAction["{PET}"], State[0], State[2]] = new HashSet<Literal> { },

                [CompoundAction["{PEEK, PET}"], State[0], State[0]] = new HashSet<Literal> { Literal["alive"], Literal["purring"] },
                [CompoundAction["{PEEK, PET}"], State[0], State[1]] = new HashSet<Literal> { Literal["alive"], Literal["~purring"] },
                [CompoundAction["{PEEK, PET}"], State[0], State[2]] = new HashSet<Literal> { Literal["~alive"], Literal["~purring"] },

                [CompoundAction["{PEEK}"], State[1], State[0]] = new HashSet<Literal> { Literal["alive"], Literal["purring"] },
                [CompoundAction["{PEEK}"], State[1], State[1]] = new HashSet<Literal> { Literal["alive"], Literal["~purring"] },
                [CompoundAction["{PEEK}"], State[1], State[2]] = new HashSet<Literal> { Literal["~alive"], Literal["~purring"] },

                [CompoundAction["{PET}"], State[1], State[0]] = new HashSet<Literal> { Literal["purring"] },
                [CompoundAction["{PET}"], State[1], State[1]] = new HashSet<Literal> { },
                [CompoundAction["{PET}"], State[1], State[2]] = new HashSet<Literal> { },

                [CompoundAction["{PEEK, PET}"], State[1], State[0]] = new HashSet<Literal> { Literal["alive"], Literal["purring"] },
                [CompoundAction["{PEEK, PET}"], State[1], State[1]] = new HashSet<Literal> { Literal["alive"], Literal["~purring"] },
                [CompoundAction["{PEEK, PET}"], State[1], State[2]] = new HashSet<Literal> { Literal["~alive"], Literal["~purring"] },

                [CompoundAction["{PEEK}"], State[2], State[0]] = new HashSet<Literal> { },
                [CompoundAction["{PEEK}"], State[2], State[1]] = new HashSet<Literal> { },
                [CompoundAction["{PEEK}"], State[2], State[2]] = new HashSet<Literal> { },

                [CompoundAction["{PET}"], State[2], State[0]] = new HashSet<Literal> { },
                [CompoundAction["{PET}"], State[2], State[1]] = new HashSet<Literal> { },
                [CompoundAction["{PET}"], State[2], State[2]] = new HashSet<Literal> { },

                [CompoundAction["{PEEK, PET}"], State[2], State[0]] = new HashSet<Literal> { },
                [CompoundAction["{PEEK, PET}"], State[2], State[1]] = new HashSet<Literal> { },
                [CompoundAction["{PEEK, PET}"], State[2], State[2]] = new HashSet<Literal> { }
            };

        private static Dictionary<(CompoundAction, State), HashSet<State>> res =
            new Dictionary<(CompoundAction, State), HashSet<State>>
            {
                {(CompoundAction["{PEEK}"], State[0]), new HashSet<State> {State[0], State[1], State[2]}},
                {(CompoundAction["{PET}"], State[0]), new HashSet<State> {State[0]}},
                {(CompoundAction["{PEEK, PET}"], State[0]), new HashSet<State> {State[0], State[1], State[2]}},

                {(CompoundAction["{PEEK}"], State[1]), new HashSet<State> {State[0], State[1], State[2]}},
                {(CompoundAction["{PET}"], State[1]), new HashSet<State> {State[0]}},
                {(CompoundAction["{PEEK, PET}"], State[1]), new HashSet<State> {State[0], State[1], State[2]}},

                {(CompoundAction["{PEEK}"], State[2]), new HashSet<State> {State[2]}},
                {(CompoundAction["{PET}"], State[2]), new HashSet<State> {State[2]}},
                {(CompoundAction["{PEEK, PET}"], State[2]), new HashSet<State> {State[2]}}
            };
    }
}
