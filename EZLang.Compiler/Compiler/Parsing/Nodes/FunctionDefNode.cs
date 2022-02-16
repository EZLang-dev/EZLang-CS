using EZLang.Compiler.Lexing;

namespace EZLang.Compiler.Parsing.Nodes;

public class FunctionDefNode : Node
{
    public FunctionDefNode()
    {
        Type = NodeType.FunctionDefiniton;
    }
}