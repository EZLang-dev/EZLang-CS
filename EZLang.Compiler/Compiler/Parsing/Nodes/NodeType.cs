namespace EZLang.Compiler.Parsing.Nodes;

public enum NodeType
{
    Null,
    Undefined,
    Token,
    Program,
    Expression,
    Definition,
    Include,
    Globals,
    Block,
    Identifier,
    FunctionDefinition,
    IfStatement,
    Call
}