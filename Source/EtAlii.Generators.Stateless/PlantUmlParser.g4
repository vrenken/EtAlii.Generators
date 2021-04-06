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

namespace                          : id (DOT id)*;
stateless_setting_namespace        : STATELESS_SETTING_NAMESPACE WHITESPACE+ namespace;
stateless_setting_class            : STATELESS_SETTING_CLASS WHITESPACE+ name=id;
stateless_setting_generate_partial : STATELESS_SETTING_GENERATE_PARTIAL;
stateless_setting_using            : STATELESS_SETTING_USING WHITESPACE+ namespace;
stateless_setting
    : stateless_setting_namespace
    | stateless_setting_class
    | stateless_setting_generate_partial
    | stateless_setting_using
    ;
state_definition_no_substates   : STATE WHITESPACE+ name=id ;
state_definition_with_substates : STATE WHITESPACE+ name=id WHITESPACE* LBRACE WHITESPACE* NEWLINE+ (WHITESPACE* (states) NEWLINE+)* WHITESPACE* RBRACE;
state_definition
    : state_definition_with_substates
    | state_definition_no_substates
    ;

states_transition_start_to      : BOUNDARY_NODE WHITESPACE* transition_from_to WHITESPACE* (to=id) trigger_details? transition_details?;
states_transition_to_start      : (to=id) WHITESPACE* transition_to_from WHITESPACE* BOUNDARY_NODE trigger_details? transition_details?;
states_transition_from_end      : (from=id) WHITESPACE* transition_from_to WHITESPACE* BOUNDARY_NODE trigger_details? transition_details?;
states_transition_end_from      : BOUNDARY_NODE WHITESPACE* transition_to_from WHITESPACE* (from=id) trigger_details? transition_details?;
states_transition_from_to       : (from=id) WHITESPACE* transition_from_to WHITESPACE* (to=id) trigger_details? transition_details?;
states_transition_to_from       : (to=id) WHITESPACE* transition_to_from WHITESPACE* (from=id) trigger_details? transition_details?;
states_description              : (node=id) WHITESPACE* COLON WHITESPACE* (text=~NEWLINE)*;
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
    : MINUS+ (orientation=id)? MINUS+ RCHEVR
    | MINUS+ RCHEVR
    ;
transition_to_from
    : LCHEVR MINUS+ (orientation=id)? MINUS+
    | LCHEVR MINUS+
    ;

transition_details_description  : (~NEWLINE)+;
transition_details              : WHITESPACE* COLON WHITESPACE* trigger_name transition_details_description?;

trigger_name                    : id (id | WHITESPACE | UNDERSCORE)*;
trigger_details                 : WHITESPACE* LCHEVR LCHEVR WHITESPACE* (ASYNC WHITESPACE+)? parameters_definition? WHITESPACE* RCHEVR RCHEVR WHITESPACE* ;
parameter_type                  : id (DOT id)* (LBRACK WHITESPACE* RBRACK)? ;
parameters_definition_unnamed   : LPAREN WHITESPACE* parameter_type WHITESPACE* (COMMA WHITESPACE* parameter_type)* WHITESPACE* RPAREN ;
parameters_definition_named     : LPAREN WHITESPACE* parameter_type WHITESPACE+ id WHITESPACE* (COMMA WHITESPACE* parameter_type WHITESPACE+ id)* WHITESPACE* RPAREN ;
parameters_definition
    : parameters_definition_unnamed
    | parameters_definition_named
    ;

note_line
    : NOTE_START (~NEWLINE)+
    | NOTE_START WHITESPACE+ QUOTED_STRING WHITESPACE+ AS WHITESPACE+ id WHITESPACE?
    | NOTE_START (~NEWLINE)+ NEWLINE ((~NEWLINE)+ NEWLINE)* WHITESPACE* NOTE_END
    ;

comment_line                    : SINGLEQUOTE (~NEWLINE)*;

// Some keywords are allowed to be used as identifiers. We need to explicitly allow this.
id_valid_keywords
    : TITLE
    | STATE
    | NOTE_START
    ;
id                              : (ID | id_valid_keywords)+ ;

