﻿using System.Collections.Generic;
using Model;

namespace Test.MinimizeNew.TestCases
{
    internal class ProducerAndConsumerTransitionFunctionGenerationTestCase : ITransitionFunctionGeneratorTestCase
    {
        public Dictionary<(CompoundAction, State), HashSet<State>> ResZero => resZero;
        public Dictionary<(CompoundAction, State, State), HashSet<Fluent>> NewSets => newSets;
        public TransitionFunction TransitionFunction
        {
            get
            {
                var transitionFunction = new TransitionFunction(CompoundAction.Values, State);
                foreach (var entry in resZero)
                    transitionFunction[entry.Key.Item1, entry.Key.Item2] = entry.Value;
                return transitionFunction;
            }
        }

        private static readonly Dictionary<string, Fluent> Fluent = new Dictionary<string, Fluent>
        {
            {"BE", new Fluent("bufferEmpty")},
            {"HI", new Fluent("hasItem")}
        };

        private static readonly Dictionary<string, Action> Action = new Dictionary<string, Action>
        {
            {"PUT", new Action("PUT")},
            {"GET", new Action("GET")},
            {"CONSUME", new Action("CONSUME")}
        };

        private static readonly Dictionary<string, CompoundAction> CompoundAction =
            new Dictionary<string, CompoundAction>
            {
                {"{PUT}", new CompoundAction(new[] {Action["PUT"]})},
                {"{GET}", new CompoundAction(new[] {Action["GET"]})},
                {"{CONSUME}", new CompoundAction(new[] {Action["CONSUME"]})},
                {"{PUT, GET}", new CompoundAction(new[] {Action["PUT"], Action["GET"]})},
                {"{PUT, CONSUME}", new CompoundAction(new[] {Action["PUT"], Action["CONSUME"]})},
                {"{GET, CONSUME}", new CompoundAction(new[] {Action["GET"], Action["CONSUME"]})},
                {"{PUT, GET, CONSUME}", new CompoundAction(new[] {Action["PUT"], Action["GET"], Action["CONSUME"]})}
            };

        private static readonly State[] State =
        {
            new State(new Dictionary<Fluent, bool> {{Fluent["BE"], true}, {Fluent["HI"], true}}),
            new State(new Dictionary<Fluent, bool> {{Fluent["BE"], false}, {Fluent["HI"], true}}),
            new State(new Dictionary<Fluent, bool> {{Fluent["BE"], true}, {Fluent["HI"], false}}),
            new State(new Dictionary<Fluent, bool> {{Fluent["BE"], false}, {Fluent["HI"], false}})
        };

        private static Dictionary<(CompoundAction, State), HashSet<State>> resZero =
            new Dictionary<(CompoundAction, State), HashSet<State>>
            {
                {(CompoundAction["{PUT}"], State[0]), new HashSet<State> {State[1]}},
                {(CompoundAction["{GET}"], State[0]), new HashSet<State> {State[0]}},
                {(CompoundAction["{CONSUME}"], State[0]), new HashSet<State> {State[2]}},
                {(CompoundAction["{PUT, GET}"], State[0]), new HashSet<State> {State[1]}},
                {(CompoundAction["{PUT, CONSUME}"], State[0]), new HashSet<State> {State[3]}},
                {(CompoundAction["{GET, CONSUME}"], State[0]), new HashSet<State> {State[2]}},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[0]), new HashSet<State> {State[3]}},

                {(CompoundAction["{PUT}"], State[1]), new HashSet<State> {State[1]}},
                {(CompoundAction["{GET}"], State[1]), new HashSet<State> {State[1]}},
                {(CompoundAction["{CONSUME}"], State[1]), new HashSet<State> {State[3]}},
                {(CompoundAction["{PUT, GET}"], State[1]), new HashSet<State> {State[1]}},
                {(CompoundAction["{PUT, CONSUME}"], State[1]), new HashSet<State> {State[3]}},
                {(CompoundAction["{GET, CONSUME}"], State[1]), new HashSet<State> {State[3]}},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[1]), new HashSet<State> {State[3]}},

                {(CompoundAction["{PUT}"], State[2]), new HashSet<State> {State[3]}},
                {(CompoundAction["{GET}"], State[2]), new HashSet<State> {State[2]}},
                {(CompoundAction["{CONSUME}"], State[2]), new HashSet<State> { }},
                {(CompoundAction["{PUT, GET}"], State[2]), new HashSet<State> {State[3]}},
                {(CompoundAction["{PUT, CONSUME}"], State[2]), new HashSet<State> { }},
                {(CompoundAction["{GET, CONSUME}"], State[2]), new HashSet<State> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[2]), new HashSet<State> { }},

                {(CompoundAction["{PUT}"], State[3]), new HashSet<State> {State[3]}},
                {(CompoundAction["{GET}"], State[3]), new HashSet<State> {State[0]}},
                {(CompoundAction["{CONSUME}"], State[3]), new HashSet<State> { }},
                {(CompoundAction["{PUT, GET}"], State[3]), new HashSet<State> {State[0]}},
                {(CompoundAction["{PUT, CONSUME}"], State[3]), new HashSet<State> { }},
                {(CompoundAction["{GET, CONSUME}"], State[3]), new HashSet<State> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[3]), new HashSet<State> { }},
            };

        private static Dictionary<(CompoundAction, State, State), HashSet<Fluent>> newSets =
            new Dictionary<(CompoundAction, State, State), HashSet<Fluent>>
            {
                // Result 0
                {(CompoundAction["{PUT}"], State[0], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET}"], State[0], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[0], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET}"], State[0], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, CONSUME}"], State[0], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET, CONSUME}"], State[0], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[0], State[0]), new HashSet<Fluent> { }},

                {(CompoundAction["{PUT}"], State[1], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET}"], State[1], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[1], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET}"], State[1], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, CONSUME}"], State[1], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET, CONSUME}"], State[1], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[1], State[0]), new HashSet<Fluent> { }},

                {(CompoundAction["{PUT}"], State[2], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET}"], State[2], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[2], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET}"], State[2], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, CONSUME}"], State[2], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET, CONSUME}"], State[2], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[2], State[0]), new HashSet<Fluent> { }},

                {(CompoundAction["{PUT}"], State[3], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET}"], State[3], State[0]), new HashSet<Fluent> {Fluent["BE"], Fluent["HI"]}},
                {(CompoundAction["{CONSUME}"], State[3], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET}"], State[3], State[0]), new HashSet<Fluent> {Fluent["BE"], Fluent["HI"]}},
                {(CompoundAction["{PUT, CONSUME}"], State[3], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET, CONSUME}"], State[3], State[0]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[3], State[0]), new HashSet<Fluent> { }},

                // Result 1
                {(CompoundAction["{PUT}"], State[0], State[1]), new HashSet<Fluent> {Fluent["BE"]}},
                {(CompoundAction["{GET}"], State[0], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[0], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET}"], State[0], State[1]), new HashSet<Fluent> {Fluent["BE"]}},
                {(CompoundAction["{PUT, CONSUME}"], State[0], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET, CONSUME}"], State[0], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[0], State[1]), new HashSet<Fluent> { }},

                {(CompoundAction["{PUT}"], State[1], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET}"], State[1], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[1], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET}"], State[1], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, CONSUME}"], State[1], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET, CONSUME}"], State[1], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[1], State[1]), new HashSet<Fluent> { }},

                {(CompoundAction["{PUT}"], State[2], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET}"], State[2], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[2], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET}"], State[2], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, CONSUME}"], State[2], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET, CONSUME}"], State[2], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[2], State[1]), new HashSet<Fluent> { }},

                {(CompoundAction["{PUT}"], State[3], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET}"], State[3], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[3], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET}"], State[3], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, CONSUME}"], State[3], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET, CONSUME}"], State[3], State[1]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[3], State[1]), new HashSet<Fluent> { }},

                // Result 2
                {(CompoundAction["{PUT}"], State[0], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET}"], State[0], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[0], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET}"], State[0], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, CONSUME}"], State[0], State[2]), new HashSet<Fluent> {Fluent["BE"], Fluent["HI"]}},
                {(CompoundAction["{GET, CONSUME}"], State[0], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[0], State[2]), new HashSet<Fluent> {Fluent["BE"], Fluent["HI"]}},

                {(CompoundAction["{PUT}"], State[1], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET}"], State[1], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[1], State[2]), new HashSet<Fluent> {Fluent["HI"]}},
                {(CompoundAction["{PUT, GET}"], State[1], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, CONSUME}"], State[1], State[2]), new HashSet<Fluent> {Fluent["HI"]}},
                {(CompoundAction["{GET, CONSUME}"], State[1], State[2]), new HashSet<Fluent> {Fluent["HI"]}},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[1], State[2]), new HashSet<Fluent> {Fluent["HI"]}},

                {(CompoundAction["{PUT}"], State[2], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET}"], State[2], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[2], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET}"], State[2], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, CONSUME}"], State[2], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET, CONSUME}"], State[2], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[2], State[2]), new HashSet<Fluent> { }},

                {(CompoundAction["{PUT}"], State[3], State[2]), new HashSet<Fluent> {Fluent["BE"]}},
                {(CompoundAction["{GET}"], State[3], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[3], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET}"], State[3], State[2]), new HashSet<Fluent> {Fluent["BE"]}},
                {(CompoundAction["{PUT, CONSUME}"], State[3], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET, CONSUME}"], State[3], State[2]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[3], State[2]), new HashSet<Fluent> { }},

                // Result 3
                {(CompoundAction["{PUT}"], State[0], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET}"], State[0], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[0], State[3]), new HashSet<Fluent> {Fluent["HI"]}},
                {(CompoundAction["{PUT, GET}"], State[0], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, CONSUME}"], State[0], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET, CONSUME}"], State[0], State[3]), new HashSet<Fluent> {Fluent["HI"]}},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[0], State[3]), new HashSet<Fluent> { }},

                {(CompoundAction["{PUT}"], State[1], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET}"], State[1], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[1], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET}"], State[1], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, CONSUME}"], State[1], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET, CONSUME}"], State[1], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[1], State[3]), new HashSet<Fluent> { }},

                {(CompoundAction["{PUT}"], State[2], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET}"], State[2], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[2], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET}"], State[2], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, CONSUME}"], State[2], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET, CONSUME}"], State[2], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[2], State[3]), new HashSet<Fluent> { }},

                {(CompoundAction["{PUT}"], State[3], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET}"], State[3], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{CONSUME}"], State[3], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET}"], State[3], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, CONSUME}"], State[3], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{GET, CONSUME}"], State[3], State[3]), new HashSet<Fluent> { }},
                {(CompoundAction["{PUT, GET, CONSUME}"], State[3], State[3]), new HashSet<Fluent> { }},
            };
    }
}