namespace EZLang.Compiler.Parsing.Nodes;

public class GlobalsNode : Node
{
    public GlobalsNode()
    {
        this.Type = NodeType.Globals;
        this.canHaveChildren = true;
    }
}