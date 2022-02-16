using EZLang.Compiler.Lexing;

namespace EZLang.Compiler.Parsing.Nodes;

public class TokenNode : Node
{

    public Token value;
    
    public TokenNode(Token value)
    {
        this.canHaveChildren = false;
        this.value = value;
        this.Type = NodeType.Token;
    }

    public override string GetTreeName()
    {
        return value.ToString();
    }
}