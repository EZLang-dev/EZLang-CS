namespace EZLang.Compiler.Parsing.Nodes;

public class IfNode : Node
{
    public IfNode()
    {
        Type = NodeType.IfStatement;
    }
}