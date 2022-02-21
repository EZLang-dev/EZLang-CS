namespace EZLang.Compiler.Parsing.Nodes;

public class ExpressionNode : Node
{
    public ExpressionNode()
    {
        Type = NodeType.Expression;
        CanHaveChildren = true;
    }
}