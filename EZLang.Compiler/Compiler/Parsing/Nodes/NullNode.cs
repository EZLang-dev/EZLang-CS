namespace EZLang.Compiler.Parsing.Nodes;

public class NullNode : Node
{
    public NullNode()
    {
        this.Type = NodeType.Null;
        this.CanHaveChildren = false;
    }
}