using EZLang.Compiler.Language;
using EZLang.Compiler.Lexing;
using EZLang.Compiler.Parsing.Nodes;
using EZLang.Diagnostics;

namespace EZLang.Compiler.Parsing;

public class Parser
{
    
    private Token[] Tokens = Array.Empty<Token>();
    private int ptr = -1;

    public List<Node> Nodes = new List<Node>();

    private bool EndOfFile()
    {
        return EndOfFileAt(0);
    }

    private bool EndOfFileAt(int offset)
    {
        return ptr + offset >= Tokens.Length;
    }

    private Token Current()
    {
        return Peek(0);
    }

    private Token Peek(int amt)
    {
        if (EndOfFileAt(amt))
        {
            return new Token(Tokens.Last().Position, TokenType.EndOfFile);
        }

        return Tokens[ptr + amt];
    }

    private void Step(int amt)
    {
        ptr += amt;
        
    }

    private Token Consume(TokenType type)
    {
        if (Current().Type != type)
        {
            Logger.Throw(ErrorType.UnexpectedToken, "Got " + Current() + ", Expected " + type, Current().Position);
            Step(1);
            return Token.NULL;
        }

        Token t = Current();
        Step(1);
        return t;
    }
    
    private Token ConsumeGroup(TokenType type, TokenType[] group)
    {
        if (!Groups.InGroup(type, group))
        {
            Logger.Throw(ErrorType.UnexpectedTokenGroup, Current().Type + " did not fit into group " + type, Current().Position);
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
        this.Tokens = tokens;
        this.ptr = 0;

        this.Nodes = new List<Node>();

        while (!EndOfFile())
        {
            Nodes.Add(ParseNode());
            Step(1);
        }

    }

    private Node ParseNode(bool expression = false)
    {
        Token c = Current();
        if (c.Type == TokenType.ProgramKwd) return ParseProgram();
        if (c.Type == TokenType.GlobalsKwd) return ParseGlobals();
        if (c.Type == TokenType.OpenBracket) return ParseBrackets();
        if (c.Type == TokenType.IncludeKwd) return ParseInclude();
        if (c.Type == TokenType.Identifier) return ParseIdentifier(Node.Null);
        if (c.Type == TokenType.FunctionKwd) return ParseFunctionDef();
        if (c.Type == TokenType.IfKwd) return ParseIfStatement();
        if (c.Type == TokenType.Newline) { Step(1); return ParseNode(); }

        if (Groups.InGroup(c.Type, Groups.GROUP_LITERALS) && !expression) return ParseExpression();
        if (Groups.InGroup(c.Type, Groups.GROUP_LITERALS) && expression) { Step(1); return new TokenNode(Peek(-1)); }
        
        Logger.Throw( ErrorType.InvalidToken, c.Type.ToString(), c.Position);
        Step(1);
        return new TokenNode(c);
    }
    
    
    /// <summary>
    /// Parse a BEDMAS expression.
    /// This function is indeed most possibly recursive!
    /// </summary>
    /// <returns>The expression node</returns>
    
    private Node ParseExpression()
    {
        ExpressionNode node = new ExpressionNode();

        node.Add(ParseNode(true), "left");

        if (Current().Type == TokenType.Operation)
        {
            Token operation = Consume(TokenType.Operation);
            int strength = GetOperatorPrecedence(operation.Value);
            node.Add(new TokenNode(operation), "operation");
            Node right = ParseNode();

            if (right.Children.ContainsKey("operation"))
            {
                Logger.Throw(ErrorType.DebugMessage, "Parsing operation");
                Token rightOp = ((TokenNode) right.Children["operation"]).value;

                if (GetOperatorPrecedence(rightOp.Value) > strength)
                {
                    
                    node.Add(right.Children["left"], "right");
                    right.Remove("left");
                    
                    right.Add(node, "right");
                    right.PrintPretty();
                    return right;
                }
            }
            
            
        }

        return node;
    }
    
    /// <summary>
    /// The thing responible for calculating BEDMAS
    /// </summary>
    /// <param name="op"></param>
    /// <returns></returns>

    private static int GetOperatorPrecedence(string op)
    {
        //TODO: Fix brackets, they are not included here.

        return op switch
        {
            "/" => 4,
            "*" => 4,
            "+" => 1,
            "-" => 1,
            _ => 0
        };
    }

    private Node ParseIfStatement()
    {
        Logger.Throw(ErrorType.WorkInProgressWarning, "If statements are not implemented yet!", Current().Position);
        IfNode node = new IfNode();
        Consume(TokenType.IfKwd);
        Consume(TokenType.OpenParenthesis);
        node.Add(ParseNode(), "statement");
        Consume(TokenType.CloseParenthesis);
        return node.Add(ParseBrackets(), "code");
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

    private Node ParseIdentifier(Node parent)
    {
        Token identifier = Consume(TokenType.Identifier);
        var c = Current();
        //LOGGER.Log(c.ToString());
        if (c.Type == TokenType.Accessor)
        {
            IdentifierNode node = new IdentifierNode();
            node.Add(new TokenNode(identifier), "object").Add(ParseAccessor(parent), "child");
            return node;
        }

        if (c.Type == TokenType.OpenParenthesis)
            return ParseFunctionCall().Add((Node)parent.ClimbToTopNode(NodeType.Identifier), "name");

        return new TokenNode(identifier);
    }

    private Node ParseFunctionCall()
    {
        CallNode node = new CallNode();
        Consume(TokenType.OpenParenthesis);
        var pcount = 0;
        do
        {
            node.Add(ParseNode(), "param[" + pcount + "]");
            pcount++;
        } 
        while (Current().Type != TokenType.CloseParenthesis);
        
        Consume(TokenType.CloseParenthesis);
        
        return node;
    }

    private Node ParseAccessor(Node parent)
    {
        Consume(TokenType.Accessor);

        return ParseIdentifier(parent);
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

        while (Peek(1).Type != TokenType.CloseBracket)
        {
            if (Current().Type == TokenType.EndOfFile)
            {
                Logger.Throw(ErrorType.EndOfFileException, "Expected \"}\", got EOF", Current().Position);
                break;
            }
            if (Current().Type == TokenType.CloseBracket)
            {
                break;
            }
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