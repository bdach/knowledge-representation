grammar DynamicSystem;

TRUTH: 'T';
FALSITY: 'F';
NEGATION: '~';
IMPLICATION: '->';
EQUIVALENCE: '<->';
CONJUNCTION: '&';
ALTERNATIVE: '|';

INITIALLY: 'initially';
AFTER: 'after';
OBSERVABLE: 'observable';
CAUSES: 'causes';
IMPOSSIBLE : 'impossible';
RELEASES: 'releases';
ALWAYS: 'always';
NONINERTIAL: 'noninertial';
IF: 'if';

TEXT: [a-zA-Z]+;

fluent: TEXT;
action: TEXT;

WS : (' ' | '\t')+ -> channel(HIDDEN);

constant: TRUTH | FALSITY;
literal: NEGATION fluent | fluent;

formula: formula EQUIVALENCE implication | implication;
implication: implication IMPLICATION alternative | alternative;
alternative: alternative ALTERNATIVE conjunction | conjunction;
conjunction: conjunction CONJUNCTION negation | negation;
negation: constant | literal | '(' formula ')' | NEGATION '(' formula ')';
condition: formula;

initialValueStatement: INITIALLY formula;

valueStatement: formula AFTER action;

observationStatement: OBSERVABLE formula AFTER action;

effectStatement: action CAUSES formula IF condition
        | IMPOSSIBLE action IF condition
        | action CAUSES formula;

fluentReleaseStatement: action RELEASES fluent IF condition
        | action RELEASES fluent;

constraintStatement: ALWAYS formula;

fluentSpecificationStatement: NONINERTIAL fluent;

actionDomain: statement*;
statement: constraintStatement
    | effectStatement
    | fluentReleaseStatement
    | fluentSpecificationStatement
    | initialValueStatement
    | observationStatement
    | valueStatement;