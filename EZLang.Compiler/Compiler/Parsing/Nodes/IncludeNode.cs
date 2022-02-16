namespace EZLang.Compiler.Parsing.Nodes;

public class IncludeNode : Node
{
    public IncludeNode()
    {
        Type = NodeType.Include;
    }
}