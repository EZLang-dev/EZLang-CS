namespace EZLang.Compiler.Lexing;

public enum TokenType
{
    Null,
    Identifier,
    Number,
    String,
    OpenBracket,
    CloseBracket,
    OpenParenthesis,
    CloseParenthesis,
    Operation,
    Accessor,
    Set,
    
    // Language
    
    IncludeKwd,
    ProgramKwd,
    FunctionKwd,
    GlobalsKwd,
    IfKwd,
    ElseKwd,
    ForKwd

}