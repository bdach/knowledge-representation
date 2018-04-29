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


initially_stmt: INITIALLY formula;
after_stmt: formula AFTER action;
observation_stmt: OBSERVABLE formula AFTER action;
effect_stmt: action CAUSES formula IF formula
        | IMPOSSIBLE action IF formula
        | action CAUSES formula;
release_stmt: action RELEASES fluent IF formula
        | action RELEASES fluent;
constraint_stmt: ALWAYS formula;
noninertial_stmt: NONINERTIAL fluent;

action_domain: statement*;
statement: initially_stmt
    | after_stmt
    | observation_stmt
    | effect_stmt
    | release_stmt
    | conjunction
    | noninertial_stmt;