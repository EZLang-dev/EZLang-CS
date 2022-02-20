namespace EZLang.Compiler.Parsing.Nodes;

public class CallNode : Node
{
    public CallNode()
    {
        Type = NodeType.Call;
    }
}