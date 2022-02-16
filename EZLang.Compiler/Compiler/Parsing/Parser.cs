using EZLang.Compiler.Lexing;
using EZLang.Compiler.Parsing.Nodes;
using OpenAbility.Debug;

namespace EZLang.Compiler.Parsing;

public class Parser
{
    
    private Token[] tokens;
    private int ptr;

    public List<Node> Nodes;
    
    private Logger LOGGER = Logger.GetLogger("Parser");
    
    private bool EndOfFile()
    {
        return EndOfFileAt(0);
    }

    private bool EndOfFileAt(int offset)
    {
        return ptr + offset >= tokens.Length;
    }

    private Token Current()
    {
        return Peek(0);
    }

    private Token Peek(int amt)
    {
        if (EndOfFileAt(amt))
        {
            return Token.NULL;
        }

        return tokens[ptr + amt];
    }

    private void Step(int amt)
    {
        ptr += amt;
        
    }

    private Token Consume(TokenType type)
    {
        if (Current().Type != type)
        {
            LOGGER.Error(Current().Position + " Unexpected token " + Current().Type);
            Step(1);
            return Token.NULL;
        }

        Token t = Current();
        Step(1);
        return t;
    }
    
    public void Parse(List<Token> tokens) => Parse(tokens.ToArray());
    public void Parse(Token[] tokens)
    {
        this.tokens = tokens;
        this.ptr = 0;

        this.Nodes = new List<Node>();

        while (!EndOfFile())
        {
            Nodes.Add(ParseNode());
            Step(1);
        }

    }

    private Node ParseNode()
    {
        if (Current().Type == TokenType.ProgramKwd) return ParseProgram();
        if (Current().Type == TokenType.GlobalsKwd) return ParseGlobals();
        if (Current().Type == TokenType.OpenBracket) return ParseBrackets();
        if (Current().Type == TokenType.IncludeKwd) return ParseInclude();
        if (Current().Type == TokenType.Identifier) return ParseIdentifier();
        if (Current().Type == TokenType.FunctionKwd) return ParseFunctionDef();
        
        LOGGER.Error( Current().Position + " Invalid token " + Current().Type);
        Step(1);
        return Node.NULL;
    }

    private Node ParseFunctionDef()
    {
        Consume(TokenType.FunctionKwd);
        Token name = Consume(TokenType.Identifier);
        
        //TODO(Jimmy): Parse arguments as well
        Consume(TokenType.OpenParenthesis);
        Consume(TokenType.CloseParenthesis);

        return new FunctionDefNode().Add(new TokenNode(name), "name").Add(ParseBrackets(), "code");

    }

    private Node ParseIdentifier()
    {
        Token identifier = Consume(TokenType.Identifier);
        var c = Current();
        if (c.Type == TokenType.Accessor)
            return new IdentifierNode().Add(new TokenNode(identifier), "object").Add(ParseAccessor(), "child");

        return new TokenNode(identifier);
    }

    private Node ParseAccessor()
    {
        Consume(TokenType.Accessor);

        return ParseIdentifier();
    }

    private Node ParseInclude()
    {
        Consume(TokenType.IncludeKwd);
        return new IncludeNode().Add(new TokenNode(Consume(TokenType.Identifier)), "package");
    }

    private Node ParseBrackets()
    {
        Consume(TokenType.OpenBracket);
        BlockNode node = new BlockNode();

        while (Current().Type != TokenType.CloseBracket)
        {
            node.Add(ParseNode());
        }

        Consume(TokenType.CloseBracket);
        
        return node;
    }

    private Node ParseProgram()
    {
        Consume(TokenType.ProgramKwd);
        Node programNode = new ProgramNode();
        
        programNode.Add(ParseBrackets(), "code");

        return programNode;
    }
    
    private Node ParseGlobals()
    {
        Consume(TokenType.GlobalsKwd);
        Node programNode = new GlobalsNode();
        
        programNode.Add(ParseBrackets(), "definitions");

        return programNode;
    }
}