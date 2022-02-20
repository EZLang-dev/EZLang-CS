namespace EZLang.Compiler.Parsing.Nodes;

public enum NodeType
{
    Null,
    Undefined,
    Token,
    Program,
    Operation,
    Definition,
    Include,
    Globals,
    Block,
    Identifier,
    FunctionDefinition,
    IfStatement,
    Call
}