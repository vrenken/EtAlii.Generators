﻿parser grammar PlantUmlParser;

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

model:
    (NEWLINE | WHITESPACE)* START WHITESPACE* NEWLINE
    (WHITESPACE* header_items? NEWLINE+)*
    (WHITESPACE* model_items NEWLINE+)*
    END (NEWLINE | WHITESPACE)*
    EOF;

title                   : TITLE (~NEWLINE)* ;
header_items
    : HIDE_EMPTY_DESCRIPTION
    | note
    | setting
    | comment
    | title
    ;

namespace                : id (DOT id)*;
setting_namespace        : SETTING_NAMESPACE WHITESPACE+ namespace;
setting_dbcontext        : SETTING_DBCONTEXT WHITESPACE+ name=id;
setting_interface        : SETTING_INTERFACE WHITESPACE+ name=id;
setting_entity           : SETTING_ENTITY WHITESPACE+ name=id;
setting_generate_partial : SETTING_GENERATE_PARTIAL;
setting_using            : SETTING_USING WHITESPACE+ namespace;
setting
    : setting_namespace
    | setting_dbcontext
    | setting_entity
    | setting_interface
    | setting_generate_partial
    | setting_using
    ;

model_items
    : relation
    | class
    | note
    | comment
    ;

relation_cardinality
    : RELATION_CARDINALITY_NONE_OR_ONE
    | RELATION_CARDINALITY_NONE_OR_MORE
    | RELATION_CARDINALITY_ONE
    | RELATION_CARDINALITY_ONE_OR_MORE
    ;
relation_type
    : RELATION_TYPE_LINK
    | RELATION_TYPE_COMPOSITION_TO_FROM
    | RELATION_TYPE_COMPOSITION_FROM_TO
    | RELATION_TYPE_AGGREGATION_TO_FROM
    | RELATION_TYPE_AGGREGATION_FROM_TO
    | RELATION_TYPE_EXTENSION_TO_FROM
    | RELATION_TYPE_EXTENSION_FROM_TO
    ;
relation_mapping        : MAP WHITESPACE+ SINGLEQUOTE from=id SINGLEQUOTE WHITESPACE+ SINGLEQUOTE to=id SINGLEQUOTE;
relation                : (relation_mapping WHITESPACE* NEWLINE WHITESPACE*)? from=id WHITESPACE+ from_cardinality=relation_cardinality WHITESPACE+ relation_type WHITESPACE+ to_cardinality=relation_cardinality WHITESPACE+ to=id (WHITESPACE* COLON WHITESPACE* property=id)?;

class_mapping           : MAP WHITESPACE+ SINGLEQUOTE name=id SINGLEQUOTE;
class_property_array    : LBRACK WHITESPACE* RBRACK;
class_property          : WHITESPACE* (PLUS|MINUS)? WHITESPACE* name=id WHITESPACE* COLON WHITESPACE* type=id WHITESPACE* is_array=class_property_array? WHITESPACE*  NEWLINE;
class                   : (class_mapping WHITESPACE* NEWLINE WHITESPACE*)? ClASS WHITESPACE+ name=id (WHITESPACE|NEWLINE)* LBRACE WHITESPACE* NEWLINE class_property* WHITESPACE* RBRACE ;
note
    : NOTE_START (~NEWLINE)+
    | NOTE_START WHITESPACE+ QUOTED_STRING WHITESPACE+ AS WHITESPACE+ id WHITESPACE?
    | NOTE_START (~NEWLINE)+ NEWLINE ((~NEWLINE)+ NEWLINE)* WHITESPACE* NOTE_END
    ;

comment                 : SINGLEQUOTE (~NEWLINE)*;

// Some keywords are allowed to be used as identifiers. We need to explicitly allow this.
id_valid_keywords
    : TITLE
    | NOTE_START
    ;
id                      : (ID | id_valid_keywords)+ ;

