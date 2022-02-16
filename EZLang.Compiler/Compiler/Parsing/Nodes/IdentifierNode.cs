namespace EZLang.Compiler.Parsing.Nodes;

public class IdentifierNode : Node
{
    public IdentifierNode()
    {
        Type = NodeType.Identifier;
    }
}