parser grammar PlantUmlParser;

@header {
    #pragma warning disable CS0115 // CS0115: no suitable method found to override
    #pragma warning disable CS3021 // CS3021: The CLSCompliant attribute is not needed because the assembly does not have a CLSCompliant attribute
    // ReSharper disable InvalidXmlDocComment
    // ReSharper disable all
}

options {
     language = CSharp;
     tokenVocab = PlantUmlLexer;
}

state_machine:
    (NEWLINE | WHITESPACE)* START WHITESPACE* NEWLINE
    (WHITESPACE* header_lines? NEWLINE+)*
    (WHITESPACE* states NEWLINE+)*
    END (NEWLINE | WHITESPACE)*
    EOF;

header_title                       : TITLE (~NEWLINE)* ;
header_lines
    : HIDE_EMPTY_DESCRIPTION
    | note_line
    | stateless_setting
    | comment_line
    | header_title
    ;

namespace                          : ID (DOT ID)*;
stateless_setting_namespace        : STATELESS_SETTING_NAMESPACE WHITESPACE+ namespace;
stateless_setting_class            : STATELESS_SETTING_CLASS WHITESPACE+ name=ID;
stateless_setting_generate_partial : STATELESS_SETTING_GENERATE_PARTIAL;
stateless_setting_using            : STATELESS_SETTING_USING WHITESPACE+ namespace;
stateless_setting
    : stateless_setting_namespace
    | stateless_setting_class
    | stateless_setting_generate_partial
    | stateless_setting_using
    ;
state_definition                : STATE WHITESPACE+ name=ID WHITESPACE* LBRACE WHITESPACE* NEWLINE+ (WHITESPACE* (states) NEWLINE+)* WHITESPACE* RBRACE;
states_transition_start_to      : BOUNDARY_NODE WHITESPACE* transition_from_to WHITESPACE* (to=ID) transition_details?;
states_transition_to_start      : (to=ID) WHITESPACE* transition_to_from WHITESPACE* BOUNDARY_NODE transition_details?;
states_transition_from_end      : (from=ID) WHITESPACE* transition_from_to WHITESPACE* BOUNDARY_NODE transition_details?;
states_transition_end_from      : BOUNDARY_NODE WHITESPACE* transition_to_from WHITESPACE* (from=ID) transition_details?;
states_transition_from_to       : (from=ID) WHITESPACE* transition_from_to WHITESPACE* (to=ID) transition_details?;
states_transition_to_from       : (to=ID) WHITESPACE* transition_to_from WHITESPACE* (from=ID) transition_details?;
states_description              : (node=ID) WHITESPACE* COLON WHITESPACE* (text=~NEWLINE)*;
states
    : states_transition_from_to
    | states_transition_to_from
    | states_transition_start_to
    | states_transition_to_start
    | states_transition_from_end
    | states_transition_end_from
    | states_description
    | state_definition
    | note_line
    | comment_line
    ;

transition_from_to
    : MINUS+ (orientation=ID)? MINUS+ RCHEVR
    | MINUS+ RCHEVR
    ;
transition_to_from
    : LCHEVR MINUS+ (orientation=ID)? MINUS+
    | LCHEVR MINUS+
    ;

transition_details_description  : (~NEWLINE)+;
transition_details              : WHITESPACE* trigger_details? COLON WHITESPACE* trigger_name transition_details_description?;

trigger_name                    : ID (ID | WHITESPACE | UNDERSCORE)*;
trigger_details                 : LCHEVR LCHEVR WHITESPACE* (ASYNC WHITESPACE+)? parameters_definition? WHITESPACE* RCHEVR RCHEVR WHITESPACE* ;
parameter_type                  : ID (LBRACK WHITESPACE* RBRACK)? ;
parameters_definition_unnamed   : LPAREN WHITESPACE* parameter_type WHITESPACE* (COMMA WHITESPACE* parameter_type)* WHITESPACE* RPAREN ;
parameters_definition_named     : LPAREN WHITESPACE* parameter_type WHITESPACE+ ID WHITESPACE* (COMMA WHITESPACE* parameter_type WHITESPACE+ ID)* WHITESPACE* RPAREN ;
parameters_definition
    : parameters_definition_unnamed
    | parameters_definition_named
    ;

note_line
    : NOTE (~NEWLINE)+
    | NOTE WHITESPACE+ QUOTED_STRING WHITESPACE+ AS WHITESPACE+ ID WHITESPACE?
    ;

comment_line                    : SINGLEQUOTE (~NEWLINE)*;
