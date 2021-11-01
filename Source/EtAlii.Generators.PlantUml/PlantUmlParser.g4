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

header_title                        : TITLE (~NEWLINE)* ;
header_lines
    : HIDE_EMPTY_DESCRIPTION
    | note_line
    | setting
    | comment_line
    | header_title
    ;

namespace                               : id (DOT id)*;
setting_namespace                       : SETTING_NAMESPACE WHITESPACE+ namespace;
setting_class                           : SETTING_CLASS WHITESPACE+ name=id;
setting_generate_partial                : SETTING_GENERATE_PARTIAL;
setting_generate_choices                : SETTING_GENERATE_CHOICES;
setting_using                           : SETTING_USING WHITESPACE+ namespace;
setting
    : setting_namespace
    | setting_class
    | setting_generate_partial
    | setting_generate_choices
    | setting_using
    ;

stereotype                              : STEREOTYPE_CHOICE;
state_definition_no_substates           : STATE WHITESPACE+ name=id (WHITESPACE+ LCHEVR LCHEVR WHITESPACE? stereotype WHITESPACE? RCHEVR RCHEVR)?;
state_definition_no_substates_full_name : STATE WHITESPACE+ QUOTED_STRING WHITESPACE+ AS WHITESPACE+ name=id (WHITESPACE+ LCHEVR LCHEVR WHITESPACE? stereotype WHITESPACE? RCHEVR RCHEVR)?;
state_definition_with_substates         : STATE WHITESPACE+ name=id (WHITESPACE+ LCHEVR LCHEVR WHITESPACE? stereotype WHITESPACE? RCHEVR RCHEVR)? WHITESPACE* LBRACE WHITESPACE* NEWLINE+ (WHITESPACE* (states) NEWLINE+)* WHITESPACE* RBRACE;
state_definition
    : state_definition_no_substates_full_name
    | state_definition_with_substates
    | state_definition_no_substates
    ;

states_transition_start_to              : BOUNDARY_NODE WHITESPACE* transition_from_to WHITESPACE* (to=id) trigger_details? transition_details?;
states_transition_to_start              : (to=id) WHITESPACE* transition_to_from WHITESPACE* BOUNDARY_NODE trigger_details? transition_details?;
states_transition_from_end              : (from=id) WHITESPACE* transition_from_to WHITESPACE* BOUNDARY_NODE trigger_details? transition_details?;
states_transition_end_from              : BOUNDARY_NODE WHITESPACE* transition_to_from WHITESPACE* (from=id) trigger_details? transition_details?;
states_transition_from_to               : (from=id) WHITESPACE* transition_from_to WHITESPACE* (to=id) trigger_details? transition_details?;
states_transition_to_from               : (to=id) WHITESPACE* transition_to_from WHITESPACE* (from=id) trigger_details? transition_details?;
states_description                      : (node=id) WHITESPACE* COLON WHITESPACE* (text=~NEWLINE)*;
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

transition_details_description          : (~NEWLINE)+;
transition_details                      : WHITESPACE* COLON WHITESPACE* trigger_name transition_details_description?;

trigger_name                            : id (id | WHITESPACE | UNDERSCORE)*;
trigger_details                         : WHITESPACE* LCHEVR LCHEVR WHITESPACE* (ASYNC WHITESPACE+)? parameters_definition? WHITESPACE* RCHEVR RCHEVR WHITESPACE* ;
parameter_type                          : id (DOT id)* (LBRACK WHITESPACE* RBRACK)? ;
parameters_definition_unnamed           : LPAREN WHITESPACE* parameter_type WHITESPACE* (COMMA WHITESPACE* parameter_type)* WHITESPACE* RPAREN ;
parameters_definition_named             : LPAREN WHITESPACE* parameter_type WHITESPACE+ id WHITESPACE* (COMMA WHITESPACE* parameter_type WHITESPACE+ id)* WHITESPACE* RPAREN ;
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

