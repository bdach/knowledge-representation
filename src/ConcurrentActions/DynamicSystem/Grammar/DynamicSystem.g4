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
POSSIBLY: 'possibly';
NECESSARY: 'necessary';
EXECUTABLE: 'executable';
SOMETIMES: 'sometimes';
ACCESSIBLE: 'accessible';
COMMA: ',';

OPEN_PAREN: '(';
CLOSE_PAREN: ')';
OPEN_BRACE: '{';
CLOSE_BRACE: '}';

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
negation: constant | literal | OPEN_BRACE formula CLOSE_PAREN | NEGATION OPEN_PAREN formula CLOSE_PAREN;
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


querySet: query*;
query: accessibilityQuery
	| existentialExecutabilityQuery
	| existentialValueQuery
	| generalExecutabilityQuery
	| generalValueQuery;

accessibilityQuery: ACCESSIBLE formula;
generalExecutabilityQuery: EXECUTABLE ALWAYS compoundActions;
existentialExecutabilityQuery: EXECUTABLE SOMETIMES compoundActions;
generalValueQuery: NECESSARY formula AFTER compoundActions;
existentialValueQuery: POSSIBLY formula AFTER compoundActions;

compoundAction: OPEN_BRACE actions CLOSE_BRACE;
actions: (action (COMMA action)*)?;
compoundActions: OPEN_PAREN (compoundAction (COMMA compoundAction)* )? CLOSE_PAREN;